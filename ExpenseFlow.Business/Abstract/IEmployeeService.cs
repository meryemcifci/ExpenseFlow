
using ExpenseFlow.Core.ViewModels;

namespace ExpenseFlow.Business.Abstract
{
    public interface IEmployeeService
    {
        UserProfileViewModel GetUserProfile(int userId);

        Task UpdateProfile(int userId, UpdateProfileViewModel model);

        Task UpdateProfilePhotoAsync(int userId, string profileImageUrl);

    }


}
