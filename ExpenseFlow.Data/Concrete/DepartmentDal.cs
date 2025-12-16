using ExpenseFlow.Data.Context;
using ExpenseFlow.DataAccess.Abstract;
using ExpenseFlow.Entity;

namespace ExpenseFlow.DataAccess.Concrete
{
    public class DepartmentDal : IDepartmentDal
    {
        private readonly ExpenseFlowContext _context;

        public DepartmentDal(ExpenseFlowContext context)
        {
            _context = context;
        }

        public List<Department> GetSelectableDepartments()
        {
            return _context.Departments
                   .Where(d => d.Name != "Muhasebe" && d.Name != "Yönetim")
                   .ToList();
        }

        public bool IsManagerExistsInDepartment(int departmentId)
        {
            // SQL Mantığı: 
            // 1. Users tablosundan o departmandaki kişileri bul.
            // 2. Bu kişilerin rollerine (UserRoles) git.
            // 3. Roller tablosunda (Roles) adı "Manager" olan var mı bak.

            var hasManager = (from user in _context.Users
                              join userRole in _context.UserRoles on user.Id equals userRole.UserId
                              join role in _context.Roles on userRole.RoleId equals role.Id
                              where user.DepartmentId == departmentId && role.Name == "Manager"
                              select user).Any();

            return hasManager;
        }
    }
}