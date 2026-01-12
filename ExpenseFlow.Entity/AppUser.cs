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

        public string? AboutMe { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? District { get; set; }


        public string? ProfileImageUrl { get; set; }


        // Bir çalışanın birden çok masrafı olabilir.
        public List<Expense> Expenses { get; set; }
    }
}
