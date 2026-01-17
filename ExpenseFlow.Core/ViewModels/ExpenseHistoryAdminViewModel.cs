using ExpenseFlow.Entity;

namespace ExpenseFlow.Core.ViewModels
{
    public class ExpenseHistoryAdminViewModel
    {       public int ExpenseId { get; set; }

            public string UserFullName { get; set; }
            public string DepartmentName { get; set; }

            public string CategoryName { get; set; }

            public decimal Amount { get; set; }
            public ExpenseStatus Status { get; set; }
            public PaymentStatus PaymentStatus { get; set; }
            public DateTime ExpenseDate { get; set; }

            public string ReceiptImagePath { get; set; }
        

    }
}
