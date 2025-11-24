using Microsoft.AspNetCore.Identity;

namespace ExpenseFlow.Entity
{
    public class AppUser:IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Bir çalışan BİR departmana aittir.
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        // Bir çalışanın birden çok masrafı olabilir.
        public List<Expense> Expenses { get; set; }
    }
}
