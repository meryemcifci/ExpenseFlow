using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.DTOs
{
    public class EmployeeExpenseDto
    {
        public List<Expense> Expenses { get; set; }

        public int ApprovedCount { get; set; }
        public int PendingCount { get; set; }
        public int RejectedCount { get; set; }
        public int PaidCount { get; set; }
    }
}
