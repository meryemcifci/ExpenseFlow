using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.DTOs;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ExpenseFlow.WebUI.Controllers
{
   
    public class AuthController : Controller
    { 
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signinManager;
        private readonly ExpenseFlowContext _context;


        public AuthController(IAuthService authService, ILogger<AuthController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, ExpenseFlowContext context)
        {
            _authService = authService;
            _logger = logger;
            _userManager = userManager;
            _signinManager = signinManager;
            _context = context;
        }

       

        [HttpGet]//sayfayı aç
        [AllowAnonymous]//giriş yapmayan kullanıcılar bu sayfayı görebilir.
        public IActionResult Register()
        {
            LoadDepartments();
            return View();
        }

        [HttpPost]//formu gönder
        [AllowAnonymous]//giriş yapmayan kullanıcılar bu sayfayı görebilir.
        public async Task<IActionResult> Register(RegisterDto model)
        {
            try
            {

                LoadDepartments();



                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Lütfen bilgilerinizi doğru giriniz!");
                    return View(model);
                }
                var usernameExist = await _userManager.FindByNameAsync(model.UserName);
                if (usernameExist != null)
                {
                    ModelState.AddModelError("", "Bu kullanıcı adı sistemde zaten kayıtlıdır!");
                    return View(model);
                }
                var emailExist = await _userManager.FindByEmailAsync(model.Email);
                if (emailExist != null)
                {
                    ModelState.AddModelError("", "Bu email sistemde zaten kayıtlıdır!");
                    return View(model);
                }
                //mapledim
                AppUser user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName,
                    DepartmentId = model.DepartmentId,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                };
                //Identity ile kullanıcı oluşturma
                var user_result = await _userManager.CreateAsync(user, model.Password);
                if (!user_result.Succeeded)
                {
                    
                    _logger.LogError($"ExpenseFlow_log_hata:  Auth/Register post email: {model.Email} kayıt işlemi başarısız oldu! ");
                    ModelState.AddModelError("", "KAYIT İŞLEMİNİZ BAŞARISIZ OLDU!");
                    foreach (var item in user_result.Errors)
                    {
                        switch (item.Code)
                        {
                            case "DuplicateEmail":
                                ModelState.AddModelError("", "Bu email sistemde zaten kayıtlıdır!");
                                break;
                            case "PasswordRequiresUpper":
                                ModelState.AddModelError("", "Parolada en az 1 tane büyük harf olması gereklidir ('A'-'Z')!");
                                break;
                            default:
                                ModelState.AddModelError("", item.Description);
                                break;
                        }

                    }
                    return View(model);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Employee");

                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Kaydınız oluştu! Ancak rol ataması kısmında sorun oluştu! Sistem yöneticisiyle görüşünüz!");
                    return View(model);
                }

                return RedirectToAction("Login", "Auth", new { email = model.Email });

             


            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex, $"ExpenseFlow_log_hata:  Auth/Register post email: {model.Email} ");
                ModelState.AddModelError("", "Beklenmekdik bir hata oluştu!");
                return View(model);
            }
            
        }
        private void LoadDepartments()
        {
            var departments = _context.Departments.ToList();
            ViewData["Departments"] = new SelectList(departments, "Id", "Name");
        }


        [HttpGet]//sayfayı aç
        [AllowAnonymous]//giriş yapmayan kullanıcılar bu sayfayı görebilir.
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]//formu gönder
        [AllowAnonymous]//giriş yapmayan kullanıcılar bu sayfayı görebilir.
        public async Task<IActionResult> Login(LoginDto model)
        {
            try 
            {

                var user_find = await _userManager.FindByEmailAsync(model.Email);
                if (user_find == null)
                {
                    ModelState.AddModelError("", "Bu email ile kayıtlı kullanıcı bulunamadı!");
                   _logger.LogInformation($"ExpenseFlow_log Auth/Login post email: {model.Email} kayıtlı kullanıcı bulunamadı");
                    return View(model);
                }

                //Identity ile giriş kontrolü   
                var signInResult = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (!signInResult.Succeeded)
                {
                    ModelState.AddModelError("", "Email veya şifre hatalı");
                    _logger.LogInformation($"ExpenseFlow_log Auth/Login post email: {user_find.UserName +" "+ model.Email} kayıtlı kullanıcı bilgileri yanlış girdi");

                    return View(model);
                }
                
               

                //var loginSuccess = await _authService.LoginAsync(model);
                //if (!loginSuccess)
                //{
                //    ModelState.AddModelError("", "Email veya şifre hatalı");
                //    return View(model);
                //}

                //Kullanıcı başarılı giriş yaptıysa Cookie Authentication için Claim'ler oluşturma
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Email) };
           
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Browser kapansa da giriş hatırlansın istiyorum.
                };

                //cookie oluşturduk adı =>Cookie.Name = "ExpenseFlow.Auth";
                //kullanıcı Remember me check atarsa program.cs içinde 7 gün hatırla kodu bulunuyor. ;


                //Role göre yönlendirme yapalım.

                bool isInRole = false;
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (await _userManager.IsInRoleAsync(user, "Employee"))
                {
                    isInRole = true;
                }
                else if (await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    isInRole = true;
                }
                else if (await _userManager.IsInRoleAsync(user, "Accountant"))
                {
                    isInRole = true;
                }
                else if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    isInRole = true;
                }

                if (!isInRole)
                {
                    _logger.LogError($"ExpenseFlow_log_hata Account/Login post email: {user.Email} sistemde rol ataması olmadığı için giriş yapmasına izin verilmedi!");
                    ModelState.AddModelError("", "Rolünüz atamasından emin olsun Sistem yöneticisiyle rolünüz hakkında görüşün!!");
                    return View();
                }

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    _logger.LogInformation($"ExpenseFlow_log Auth/Login post email: {user.Email} giriş yaptı!");
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else if (await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    _logger.LogInformation($"ExpenseFlow_log Auth/Login post email: {user.Email} giriş yaptı!");
                    return RedirectToAction("Index", "Manager");
                }
                else if (await _userManager.IsInRoleAsync(user, "Accountant"))
                {
                    _logger.LogInformation($"ExpenseFlow_log Auth/Login post email: {user.Email} giriş yaptı!");
                    return RedirectToAction("Index", "Accountant");
                }

                _logger.LogInformation($"ExpenseFlow_log Auth/Login post email: {user.Email} giriş yaptı!");
                return RedirectToAction("Index", "Employee");


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ExpenseFlow_log_hata:  Account/Login post email: {model} ");

                ModelState.AddModelError("", "Beklenmekdik bir hata oluştu!");
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("ExpenseFlow_log: Kullanıcı başarıyla logout oldu.");

                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ExpenseFlow_log_hata: Logout sırasında hata oluştu.");

                return RedirectToAction("Index", "Home");
            }
        }



    }
}
