using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Data.Concrete
{
    public class EmployeeDal : IEmployeeDal
    {
        
        private readonly ExpenseFlowContext _context;

        public EmployeeDal(ExpenseFlowContext context)
        {
            _context = context;
        }

      
        public UserProfileViewModel GetUserProfile(int userId)
        {
            return _context.Users
                .Where(x => x.Id == userId)
                .Select(x => new UserProfileViewModel
                {
                    UserId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,  
                    UserName = x.UserName,
                    City = x.City,
                    Country = x.Country,
                    Address = x.Address,
                    ProfileImageUrl = x.ProfileImageUrl,
                    DepartmentName = x.Department.Name
                })
                .FirstOrDefault();
        }


        public async Task UpdateProfileAsync(int userId, UpdateProfileViewModel model)
        {
            var user = await _context.Users
               .Where(u => u.Id == userId)
               .Where(u => _context.UserRoles
                   .Any(ur => ur.UserId == u.Id && ur.RoleId ==1))
               .FirstOrDefaultAsync();


            if (user == null)
                throw new Exception("Kullanıcı bulunamadı veya Employee değil");

            user.AboutMe = model.AboutMe;
            user.Address = model.Address;
            user.City = model.City;
            user.Country = model.Country;
           

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfilePhotoAsync(int userId, string profileImageUrl)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            user.ProfileImageUrl = profileImageUrl;
            await _context.SaveChangesAsync();
        }
    }
}
