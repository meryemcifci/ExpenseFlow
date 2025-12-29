using ExpenseFlow.Entity;

namespace ExpenseFlow.Data.Abstract
{
    public interface IExpenseDal
    {
        //Buraya Expense ile ilgili metotlar eklenecek
        void Insert(Expense expense);
        void Update(Expense expense);
        void Delete(int id);

        Expense GetById(int id);
        List<Expense> GetList();
        List<Expense> GetPendingWithCategory();
        List<Expense> GetApproveWithCategory();
        List<Expense> GetRejectWithCategory();
        Task UpdatePaymentStatusAsync(int expenseId, PaymentStatus status);
        List<Expense> GetApprovedExpenses();

        decimal TotalAmount(int userId);
        decimal TotalPaidAmount(int userId);
        decimal TotalPendingAmount(int userId);

        List<int> GetMonthlyExpenseCounts(int userId);
        List<int> GetWeeklyExpenseCounts(int userId);


        decimal GetTotalAmountByPaymentStatus(int userId, PaymentStatus paymentStatus);
        decimal GetTotalAmountByExpenseStatus(int userId, ExpenseStatus expenseStatus);
        List<(string CategoryName, decimal TotalAmount)> GetTotalAmountByCategory(int userId);


    }
}
