using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.DTOs;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using Microsoft.AspNetCore.Identity;

namespace ExpenseFlow.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;

        public UserService(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<UserDto> GetUserListWithDetails()
        {
            // 1. DAL'dan Tuple Olarak Veriyi Çek
            var tupleList = _userDal.GetUsersWithDetails();

            // 2. Tuple'ı UserDto'ya Çevir
            var dtoList = tupleList.Select(t => new UserDto
            {
                Id = t.User.Id.ToString(),
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,

                // Tuple'ın 2. ve 3. elemanlarını kullanıyoruz
                DepartmentName = t.DepartmentName,
                Role = t.RoleName
            }).ToList();

            return dtoList;
        }
    }
}

