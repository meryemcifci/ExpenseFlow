using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.Abstract
{
    public interface IReportService
    {
        Task<byte[]> GenerateUserExpensePdfAsync(int userId, string logoPath);

        Task<byte[]> GenerateUserExpenseExcelAsync(int userId, string logoPath);


        Task<byte[]> GenerateMonthlyManagerPdfAsync(int year, int month, string logoPath);


        Task<byte[]> GenerateMonthlyAccountantPdfAsync(int year, int month, string logoPath);






    }
    
}
