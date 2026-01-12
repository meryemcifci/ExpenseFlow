namespace ExpenseFlow.Core.ViewModels
{
    public class ExpenseListViewModel
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public DateTime Date { get; set; }

        public string CategoryName { get; set; }

        public decimal Amount { get; set; }

        public int Status { get; set; }

        public string Description { get; set; }
    }
}