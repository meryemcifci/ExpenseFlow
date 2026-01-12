using ExpenseFlow.Entity;

namespace ExpenseFlow.Data.Abstract
{
    public interface IUserDal
    {
        bool HasAccountant();
        Task<AppUser> GetByIdAsync(int id);
        List<(AppUser User, string DepartmentName, string RoleName)> GetUsersWithDetails();
        Task<List<AppUser>> GetEmployeesByDepartmentAsync(int departmentId);
    }
}
