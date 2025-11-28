using ExpenseFlow.Business.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ExpenseFlow.Business.Abstract
{
    public interface IAuthService
    {
       
        Task<IdentityResult> RegisterAsync(RegisterDto model);

        Task<bool> LoginAsync(LoginDto model);
    }
}
