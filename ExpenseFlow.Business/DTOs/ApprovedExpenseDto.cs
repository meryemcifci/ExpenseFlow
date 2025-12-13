using ExpenseFlow.Entity;

public class ApprovedExpenseDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string? CategoryName { get; set; }
    public DateTime Date { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}
