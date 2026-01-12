using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.Services;
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
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;
        private readonly IUserDal _userDal;


        public ManagerController(IExpenseService expenseService, IWebHostEnvironment env, ICategoryService categoryService, INotificationService notificationService, IEmployeeService employeeService, IUserService userService, IUserDal userDal)
        {
            _expenseService = expenseService;
            _env = env;
            _categoryService = categoryService;
            _notificationService = notificationService;
            _employeeService = employeeService;
            _userService = userService;
            _userDal = userDal;
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


        public IActionResult MANAGER()
        {
            return View();
        }


    }

}
