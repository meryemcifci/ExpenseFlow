using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Data.Abstract;

namespace ExpenseFlow.Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDal _employeeDal;

        public EmployeeService(IEmployeeDal employeeDal)
        {
            _employeeDal = employeeDal;
        }

        public UserProfileViewModel GetUserProfile(int userId)
        {
            return _employeeDal.GetUserProfile(userId);
        }

        public async Task UpdateProfile(int userId, UpdateProfileViewModel model)
        {
            await _employeeDal.UpdateProfileAsync(userId,model);
        }

        public async Task UpdateProfilePhotoAsync(int userId, string profileImageUrl)
        {
            await _employeeDal.UpdateProfilePhotoAsync(userId, profileImageUrl);
        }
    }
}
