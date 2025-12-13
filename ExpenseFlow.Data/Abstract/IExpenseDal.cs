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


    }
}
