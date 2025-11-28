namespace ExpenseFlow.Business.DTOs
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName=> $"{FirstName}{LastName}";
        public string Email { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
