using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Data.Concrete
{
    public class AccountantDal : IAccountantDal
    {
        private readonly ExpenseFlowContext _context;

        public AccountantDal(ExpenseFlowContext context)
        {
            _context = context;
        }
        public async Task<UserProfileViewModel> GetAccountantProfileAsync(int userId)
        {
            var accountant = await _context.Users
               .Include(x => x.Department)
               .FirstOrDefaultAsync(x => x.Id == userId);

            if (accountant == null)
                return null;


            return new UserProfileViewModel
            {
                UserId = accountant.Id,
                UserName = accountant.UserName,
                Email = accountant.Email,
                DepartmentName = accountant.Department.Name,
                AboutMe = accountant.AboutMe,
                ProfileImageUrl = accountant.ProfileImageUrl ?? "/img/default-avatar.png",
                Address = accountant.Address,
                City = accountant.City,
                Country = accountant.Country,
                District = accountant.District


            };
        }

        public async Task UpdateProfileAsync(int userId, UpdateProfileViewModel model)
        {
            var user = await _context.Users
               .Where(u => u.Id == userId)
               .Where(u => _context.UserRoles
                   .Any(ur => ur.UserId == u.Id && ur.RoleId == 3))
               .FirstOrDefaultAsync();


            if (user == null)
                throw new Exception("Kullanıcı bulunamadı veya Accountant değil");

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
