using ExpenseFlow.Core.ViewModels;

namespace ExpenseFlow.Business.Abstract
{
    public interface IManagerService
    {
        Task<UserProfileViewModel> GetManagerProfileAsync(int userId);


        Task UpdateProfile(int userId, UpdateProfileViewModel model);
        Task UpdateProfilePhotoAsync(int userId, string profileImageUrl);
    }
}
