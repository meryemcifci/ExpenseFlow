using ExpenseFlow.Entity;
using ExpenseFlow.Business.Abstract;
using Microsoft.AspNetCore.Identity;
using ExpenseFlow.WebUI.Models.Dtos;


namespace ExpenseFlow.Business.Services
{
    //ilk olarak implemente ederek başlayalım.
    public class AuthService:IAuthService
    {

        //Identity'nin hazır servisleri
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) //DI
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //register, login, logout, change password, reset password 

        public async Task<bool> LoginAsync(LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            return result.Succeeded;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            AppUser user = new AppUser
            {
                UserName = model.FirstName + model.LastName,
                Email = model.Email,
                DepartmentId = model.DepartmentId

            };
            var result=await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //kayıt başarılı ise kullanıcıyı "Employee" rolüne ekle daha sonra Admi
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            return result;

        }


    }
}
