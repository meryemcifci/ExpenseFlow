using ExpenseFlow.Business.DTOs;

namespace ExpenseFlow.Business.Abstract
{
    public interface IUserService
    {
        List<UserDto> GetUserListWithDetails();

        bool HasAccountant();


    }
}
