using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.Services;
using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Data.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;


namespace ExpenseFlow.WebUI.Controllers
{

    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {

        private readonly IExpenseService _expenseService;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService _categoryService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IUserDal _userDal;//UserDal buraya eklemek istemezdim :(
        private readonly IManagerService _managerService;
        private readonly IAddressService _addressService;
        private readonly IReportService _reportService;


        public ManagerController(IExpenseService expenseService, IWebHostEnvironment env, ICategoryService categoryService, IUserService userService, IUserDal userDal, IManagerService managerService, IAddressService addressService, IReportService reportService)
        {
            _expenseService = expenseService;
            _env = env;
            _categoryService = categoryService;
            _userService = userService;
            _userDal = userDal;
            _managerService = managerService;
            _addressService = addressService;
            _reportService = reportService;
        }

        public async Task<IActionResult> Index(int? employeeId, DateTime? startDate, DateTime? endDate, int? status, int? categoryId)
        {
            // Giriş yapan kullanıcının ID'sini alıyoruz
            var managerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Dashboard verilerini çektim
            var model = await _expenseService.GetDashboardExpensesAsync(employeeId, startDate, endDate, status, categoryId, managerId);

            // Dropdown için çalışan listesini çektim (Filtreleme barı için)
            var manager = await _userDal.GetByIdAsync(managerId);
            var employees = await _userService.GetEmployeesByDepartmentAsync(manager.DepartmentId);

            ViewBag.Employees = new SelectList(employees.Select(e => new { Id = int.Parse(e.Id), FullName = e.FirstName + " " + e.LastName }), "Id", "FullName");

            // Kategori listesini çektim
            var categories = _categoryService.TGetList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);

            // Kategoriye göre masraf sayılarını çektim
            var categoryCounts = await _expenseService.GetExpenseCountsByCategoryAsync(managerId);
            ViewBag.CategoryCounts = categoryCounts;

            // Özet Metrikleri.....
            ViewBag.PendingCount = model.Count(x => x.Status == 1);
            ViewBag.MonthlyTotal = model.Where(x => x.Status == 2 && x.Date.Month == DateTime.Now.Month).Sum(x => x.Amount);

            return View(model);
        }


        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var profile = await _managerService.GetManagerProfileAsync(userId);

            return View(profile);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            var userId = int.Parse(
               User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );
            await _managerService.UpdateProfile(userId, model);
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return RedirectToAction("Profile");

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/uploads/profile"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            var photoUrl = "/uploads/profile/" + fileName;

            await _managerService.UpdateProfilePhotoAsync(userId, photoUrl);

            return RedirectToAction("Profile");
        }


        [HttpGet]
        public IActionResult MonthlyReport()
        {
            return View();
        }

        // PDF İNDİRME
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> DownloadMonthlyManagerPdf(
            int year,
            int month)
        {
            string logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "img",
                "ExpenseFlow.png"
            );

            var pdfBytes = await _reportService
                .GenerateMonthlyManagerPdfAsync(year, month, logoPath);

            var fileName =
                $"ManagerExpenseReport_{year}_{month:D2}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

            return File(
                pdfBytes,
                "application/pdf",
                fileName
            );
        }


    }
}


