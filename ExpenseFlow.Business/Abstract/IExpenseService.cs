using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.Abstract
{
    public interface IExpenseService
    {
        //Buraya Expense ile ilgili metotlar eklenecek
        //CRUD işlemleri
        void TInsert(Expense expense);// masraf eklemek için
        void TUpdate(Expense expense);// masraf güncellemek için
        void TDelete(int id);// masraf silmek için
        Expense TGetById(int id);// id ye göre masraf getirmek için
        List<Expense> TGetList();// tüm masrafları listelemek için

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

        decimal GetPaidAmount(int userId);
        decimal GetPendingAmount(int userId);
        decimal GetRejectedAmount(int userId);
        List<(string CategoryName, decimal TotalAmount)> GetExpenseAmountsByCategory(int userId);

    }
}
