using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Data.Concrete
{
    public class UserDal : IUserDal
    {
        private readonly ExpenseFlowContext _context;

        public UserDal(ExpenseFlowContext context)
        {
            _context = context;
        }

        

        public List<(AppUser User, string DepartmentName, string RoleName)> GetUsersWithDetails()
        {
            var result = from user in _context.Users
                         join userRole in _context.UserRoles on user.Id equals userRole.UserId
                         join role in _context.Roles on userRole.RoleId equals role.Id
                         join dept in _context.Departments on user.DepartmentId equals dept.Id into deptGroup
                         from d in deptGroup.DefaultIfEmpty()
                             //tuple döndürüyorum.
                             //Tuple Nedir? https://learn.microsoft.com/tr-tr/dotnet/api/system.tuple-3?view=net-8.0 tuple yapısı birbirinde farklı veritiplerini tek bir nesnede tutmamızı sağlar. 
                         select new
                         {
                             UserObj = user,
                             DeptName = d != null ? d.Name : "Atanmamış",
                             RoleName = role.Name
                         };

            // LINQ to Entities (SQL) tuple'ı doğrudan desteklemez, önce belleğe (AsEnumerable) alıp sonra Tuple'a çeviriyoruz.
            return result.AsEnumerable()
                         .Select(x => (x.UserObj, x.DeptName, x.RoleName))
                         .ToList();
        }

        public bool HasAccountant()
        {
            return _context.UserRoles
            .Join(
                _context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name
            )
            .Any(roleName => roleName == "Accountant");
        }

        public async Task<AppUser> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<AppUser>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await _context.Users
                .Where(u => u.DepartmentId == departmentId)
                .ToListAsync();
        }
    }
}
