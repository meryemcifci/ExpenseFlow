using ExpenseFlow.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Data.Abstract
{
    public interface IManagerDal
    {
        Task<UserProfileViewModel> GetManagerProfileAsync(int userId);

        Task UpdateProfileAsync(int userId, UpdateProfileViewModel model);
        Task UpdateProfilePhotoAsync(int userId, string profileImageUrl);
    }
}
