using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Data.Abstract;

namespace ExpenseFlow.Business.Services
{
    public class AccountantService: IAccountantService
    {
        private readonly IAccountantDal _accountantDal;

        public AccountantService(IAccountantDal accountantDal)
        {
            _accountantDal = accountantDal;

        }

        public async Task<UserProfileViewModel> GetAccountantProfileAsync(int userId)
        {
            return await _accountantDal.GetAccountantProfileAsync(userId);
        }

        public async Task UpdateProfile(int userId, UpdateProfileViewModel model)
        {
            await _accountantDal.UpdateProfileAsync(userId, model);
        }

        public async Task UpdateProfilePhotoAsync(int userId, string profileImageUrl)
        {
            await _accountantDal.UpdateProfilePhotoAsync(userId, profileImageUrl);
        }
    }
}
