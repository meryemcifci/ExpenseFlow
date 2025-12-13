using ExpenseFlow.Entity;

namespace ExpenseFlow.Data.Abstract
{
    public interface IUserDal
    {
        List<(AppUser User, string DepartmentName, string RoleName)> GetUsersWithDetails();
        bool HasAccountant();

    }
}
