using Microsoft.AspNetCore.Http;

namespace ExpenseFlow.Business.DTOs
{
    public class CreateExpenseDto
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }

       
        public IFormFile? ReceiptFile { get; set; }
    }
}
