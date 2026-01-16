using ExpenseFlow.Core.ViewModels;

namespace ExpenseFlow.Data.Abstract
{
    public interface IAccountantDal
    {
        Task<UserProfileViewModel> GetAccountantProfileAsync(int userId);

        Task UpdateProfileAsync(int userId, UpdateProfileViewModel model);
        Task UpdateProfilePhotoAsync(int userId, string profileImageUrl);
    }
}
