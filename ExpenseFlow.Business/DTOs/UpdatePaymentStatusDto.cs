using ExpenseFlow.Entity;

public class UpdatePaymentStatusDto
{
    public int ExpenseId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}
