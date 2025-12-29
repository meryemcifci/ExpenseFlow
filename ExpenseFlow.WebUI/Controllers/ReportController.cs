using ExpenseFlow.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseFlow.WebUI.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Giriş yapan kullanıcının tüm masraflarını PDF olarak indirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DownloadUserExpensePdf()
        {
            // Giriş yapan kullanıcı Id
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

          
            string logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "img",
                "ExpenseFlow.png"
            );

            var pdfBytes = await _reportService.GenerateUserExpensePdfAsync(userId, logoPath);
            var fileName = $"ExpenseReport_{DateTime.Now:yyyyMMdd_HH.mm}.pdf";

            return File(
                pdfBytes,
                "application/pdf",
                fileName
            );
        }

        [HttpGet]
        public async Task<IActionResult> DownloadUserExpenseExcel()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            string logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "img",
                "ExpenseFlow.png"
            );
            var excelBytes = await _reportService.GenerateUserExpenseExcelAsync(userId,logoPath);
            var fileName = $"ExpenseReport_{DateTime.Now:yyyyMMdd_HH.mm}.xls";

            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

    }
}
