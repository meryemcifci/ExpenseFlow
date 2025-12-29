using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.DTOs
{
    public class EmployeeDashboardDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ExpenseStatus ExpenseStatus { get; set; }

    }
}
