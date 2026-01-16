using ExpenseFlow.Entity;

namespace ExpenseFlow.Data.Abstract
{
    public interface IReportDal
    {
        Task<List<Expense>> GetUserExpenseReportDataAsync(int userId);

        Task<List<Expense>> GetMonthlyManagerReportDataAsync(int year, int month);



    }
}
