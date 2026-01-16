using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Concrete;

namespace ExpenseFlow.Business.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerDal _managerDal;

        public ManagerService(IManagerDal managerDal)
        {
            _managerDal = managerDal;
            
        }

        public async Task<UserProfileViewModel> GetManagerProfileAsync(int userId)
        {
            return await _managerDal.GetManagerProfileAsync(userId);
        }

        public async Task UpdateProfile(int userId, UpdateProfileViewModel model)
        {
            await _managerDal.UpdateProfileAsync(userId, model);
        }

        public async Task UpdateProfilePhotoAsync(int userId, string profileImageUrl)
        {
            await _managerDal.UpdateProfilePhotoAsync(userId, profileImageUrl);
        }
    }
}
