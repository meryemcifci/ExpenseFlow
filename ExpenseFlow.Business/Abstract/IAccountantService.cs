using ExpenseFlow.Core.ViewModels;

namespace ExpenseFlow.Business.Abstract
{
    public interface IAccountantService
    {
        Task<UserProfileViewModel> GetAccountantProfileAsync(int userId);


        Task UpdateProfile(int userId, UpdateProfileViewModel model);
        Task UpdateProfilePhotoAsync(int userId, string profileImageUrl);
    }
}
