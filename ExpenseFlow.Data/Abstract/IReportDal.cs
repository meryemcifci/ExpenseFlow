using ExpenseFlow.Entity;

namespace ExpenseFlow.Data.Abstract
{
    public interface IReportDal
    {
        Task<List<Expense>> GetUserExpenseReportDataAsync(int userId);


    }
}
