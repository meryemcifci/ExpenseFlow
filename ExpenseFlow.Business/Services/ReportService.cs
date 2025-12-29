using ClosedXML.Excel;
using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpenseFlow.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportDal _reportDal;

        public ReportService(IReportDal reportDal)
        {
            _reportDal = reportDal;
        }

        // Satır rengine karar veren yardımcı metot
        private string GetRowColor(Expense expense)
        {
            return expense.Status switch
            {
                ExpenseStatus.Approved => Colors.Green.Lighten4,
                ExpenseStatus.Pending => Colors.Yellow.Lighten4,
                ExpenseStatus.Rejected => Colors.Red.Lighten4,
                _ => Colors.Grey.Lighten4
            };
        }

        public async Task<byte[]> GenerateUserExpensePdfAsync(int userId, string logoPath)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var expenses = await _reportDal.GetUserExpenseReportDataAsync(userId);

            var user = expenses.FirstOrDefault()?.User;
            var fullName = $"{user?.FirstName} {user?.LastName}";
            var departmentName = user?.Department?.Name ?? "-";

            var total = expenses.Sum(x => x.Amount);
            var paid = expenses.Where(x => x.PaymentStatus == PaymentStatus.Paid).Sum(x => x.Amount);
            var pending = expenses.Where(x => x.PaymentStatus == PaymentStatus.Pending).Sum(x => x.Amount);
            var rejected = expenses.Where(x => x.Status == ExpenseStatus.Rejected).Sum(x => x.Amount);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    //pdf başlığım
                    page.Header().PaddingBottom(10).Column(col =>
                    {
                        col.Item()
                            .AlignCenter()
                            .MaxHeight(50)
                            .Image(logoPath);

                        col.Item()
                            .AlignCenter()
                            .Text("Harcama Raporu")
                            .FontSize(13)
                            .Bold();

                        col.Item()
                            .AlignCenter()
                            .Text($"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy}")
                            .FontSize(10);
                    });

                    //içeriğim
                    page.Content().Column(col =>
                    {
                        // Kullanıcı bilgileri
                        col.Item()
                            .Background(Colors.Grey.Lighten3)
                            .Padding(8)
                            .Column(info =>
                            {
                                info.Item().Text($"Kullanıcı: {fullName}").Bold();
                                info.Item().Text($"Departman: {departmentName}").Bold();
                            });

                        // Tablo
                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Tarih").Bold();
                                header.Cell().Text("Açıklama").Bold();
                                header.Cell().Text("Kategori").Bold();
                                header.Cell().AlignRight().Text("Tutar (₺)").Bold();
                            });

                            foreach (var e in expenses)
                            {
                                var bg = GetRowColor(e);

                                table.Cell().Background(bg).Padding(4)
                                    .Text(e.Date.ToString("dd.MM.yyyy"));

                                table.Cell().Background(bg).Padding(4)
                                    .Text(e.Description ?? "-")
                                    .WrapAnywhere();

                                table.Cell().Background(bg).Padding(4)
                                    .Text(e.Category?.Name ?? "-");

                                table.Cell().Background(bg).Padding(4)
                                    .AlignRight()
                                    .Text($"{e.Amount:0.00}");
                            }
                        });

                       //özet
                        var total = expenses.Sum(x => x.Amount);
                        var paid = expenses.Where(x => x.PaymentStatus == PaymentStatus.Paid).Sum(x => x.Amount);
                        var pending = expenses.Where(x => x.PaymentStatus == PaymentStatus.Pending).Sum(x => x.Amount);
                        var rejected = expenses.Where(x => x.Status == ExpenseStatus.Rejected).Sum(x => x.Amount);

                        col.Item().PaddingTop(15).Row(row =>
                        {
                            row.RelativeItem().Background(Colors.Blue.Lighten3).Padding(10)
                                .Text($"Toplam: {total:0.00} ₺").Bold();

                            row.RelativeItem().Background(Colors.Green.Lighten3).Padding(10)
                                .Text($"Ödenen: {paid:0.00} ₺").Bold();

                            row.RelativeItem().Background(Colors.Yellow.Lighten3).Padding(10)
                                .Text($"Bekleyen: {pending:0.00} ₺").Bold();

                            row.RelativeItem().Background(Colors.Red.Lighten3).Padding(10)
                                .Text($"Reddedilen: {rejected:0.00} ₺").Bold();
                        });

                    });

                   //pdf footorım
                    page.Footer()
                        .AlignCenter()
                        .Text("ExpenseFlow © 2025")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken2);
                });
            }).GeneratePdf();

            return pdf;
        }

        public async Task<byte[]> GenerateUserExpenseExcelAsync(int userId, string logoPath)
        {
            var expenses = await _reportDal.GetUserExpenseReportDataAsync(userId);

            var user = expenses.FirstOrDefault()?.User;
            var department = user?.Department?.Name ?? "-";
            var fullName = $"{user?.FirstName} {user?.LastName}";

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Masraf Raporu");

           //logom
            if (File.Exists(logoPath))
            {
                var image = ws.AddPicture(logoPath)
                    .MoveTo(ws.Cell("B1"))
                    .WithSize(120, 120);
            }

            int row = 6;

          //head
            ws.Cell(row, 1).Value = "Kullanıcı:";
            ws.Cell(row, 2).Value = fullName;
            row++;

            ws.Cell(row, 1).Value = "Departman:";
            ws.Cell(row, 2).Value = department;
            row++;

            ws.Cell(row, 1).Value = "Rapor Tarihi:";
            ws.Cell(row, 2).Value = DateTime.Now.ToString("dd.MM.yyyy");
            row += 2;

            ws.Range("A6:A8").Style.Font.Bold = true;

            //tablo başlıklarım
            ws.Cell(row, 1).Value = "Tarih";
            ws.Cell(row, 2).Value = "Açıklama";
            ws.Cell(row, 3).Value = "Kategori";
            ws.Cell(row, 4).Value = "Tutar (₺)";
            ws.Cell(row, 5).Value = "Ödeme Durumu";
            ws.Cell(row, 6).Value = "Onay Durumu";

            ws.Range(row, 1, row, 6).Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.LightGray)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            row++;

            //harcamalar
            foreach (var e in expenses)
            {
                ws.Cell(row, 1).Value = e.Date.ToString("dd.MM.yyyy");
                ws.Cell(row, 2).Value = e.Description;
                ws.Cell(row, 3).Value = e.Category?.Name;
                ws.Cell(row, 4).Value = e.Amount;
                ws.Cell(row, 5).Value = e.PaymentStatus.ToString();
                ws.Cell(row, 6).Value = e.Status.ToString();

                row++;
            }

            row += 1;

            // özet
            ws.Cell(row, 3).Value = "Toplam:";
            ws.Cell(row, 4).Value = expenses.Sum(x => x.Amount);
            row++;

            ws.Cell(row, 3).Value = "Ödenen:";
            ws.Cell(row, 4).Value = expenses
                .Where(x => x.PaymentStatus == PaymentStatus.Paid)
                .Sum(x => x.Amount);
            row++;

            ws.Cell(row, 3).Value = "Bekleyen:";
            ws.Cell(row, 4).Value = expenses
                .Where(x => x.PaymentStatus == PaymentStatus.Pending)
                .Sum(x => x.Amount);
            row++;

            ws.Cell(row, 3).Value = "Reddedilen:";
            ws.Cell(row, 4).Value = expenses
                .Where(x => x.Status == ExpenseStatus.Rejected)
                .Sum(x => x.Amount);

            ws.Range(row - 3, 3, row, 4).Style.Font.Bold = true;

            ws.Columns().AdjustToContents();
            ws.Column(4).Style.NumberFormat.Format = "#,##0.00 ₺";

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }
    }
}
