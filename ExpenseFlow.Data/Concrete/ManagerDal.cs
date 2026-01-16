using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Data.Concrete
{
    public class ManagerDal : IManagerDal
    {
        private readonly ExpenseFlowContext _context;

        public ManagerDal(ExpenseFlowContext context)
        {
            _context = context;
        }
        public async Task<UserProfileViewModel> GetManagerProfileAsync(int userId)
        {
            var manager = await _context.Users
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (manager == null)
                return null;

            
            return new UserProfileViewModel
            {
                UserId = manager.Id,
                UserName = manager.UserName,
                Email = manager.Email,
                DepartmentName = manager.Department.Name,
                AboutMe = manager.AboutMe,
                ProfileImageUrl = manager.ProfileImageUrl ?? "/img/default-avatar.png",
                Address = manager.Address,     
                City = manager.City,          
                Country = manager.Country,     
                District = manager.District


            };
        }
        public async Task UpdateProfileAsync(int userId, UpdateProfileViewModel model)
        {
            var user = await _context.Users
               .Where(u => u.Id == userId)
               .Where(u => _context.UserRoles
                   .Any(ur => ur.UserId == u.Id && ur.RoleId == 2))
               .FirstOrDefaultAsync();


            if (user == null)
                throw new Exception("Kullanıcı bulunamadı veya Manager değil");

            user.AboutMe = model.AboutMe;
            user.Address = model.Address;
            user.City = model.City;
            user.Country = model.Country;
            user.District = model.District;


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
