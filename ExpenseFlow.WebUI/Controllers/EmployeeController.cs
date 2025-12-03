using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.DTOs;
using ExpenseFlow.Entity;
using ExpenseFlow.WebUI.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ExpenseFlow.WebUI.Controllers
{

    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        public readonly ILogger<EmployeeController> _logger;
      
        private readonly IExpenseService _expenseService;
        private readonly IWebHostEnvironment _env;
        private readonly ICategoryService _categoryService;


        public EmployeeController(ILogger<EmployeeController> logger,  IExpenseService expenseService, IWebHostEnvironment env, ICategoryService categoryService)
        {

            _logger = logger;
            _expenseService = expenseService;
            _env = env;
            _categoryService = categoryService;
        }
        //Dashboard
        public IActionResult Index()
        {
            return View();
        }
        //masraflarım buradan görüntülenecek
        public IActionResult MyExpenses()
        {
            //Todo: try catch ekle ve log ekle...
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                var expenses = _expenseService
                                .TGetList()
                                .Where(x => x.UserId == userId)
                                .ToList();

                var model = new EmployeeExpenseDto
                {
                    Expenses = expenses,
                    ApprovedCount = expenses.Count(x => x.Status == ExpenseStatus.Approved),
                    PendingCount = expenses.Count(x => x.Status == ExpenseStatus.Pending),
                    RejectedCount = expenses.Count(x => x.Status == ExpenseStatus.Rejected)
                };

                return View(model);

                
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ExpenseFlow_log_hata Employee/MyExpensee : {ex.Message} ");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }
        //masraf ekleyeceğim.
        [HttpGet]
        public IActionResult NewAddExpense()
        {
            var categories = _categoryService.TGetList();

            ViewBag.Categories = categories;

            return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> NewAddExpense(CreateExpenseDto dto)
        {

            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                var expense = new Expense
                {
                    Description = dto.Description,
                    Amount = dto.Amount,
                    CategoryId = dto.CategoryId,
                    UserId = userId

                };

                if (dto.ReceiptFile != null)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid() + Path.GetExtension(dto.ReceiptFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await dto.ReceiptFile.CopyToAsync(stream);

                    expense.ReceiptImage = "/uploads/" + fileName;
                }
                else
                {
                    _logger.LogInformation("ExpenseFlow_log_hata Employee/NewAddExpense : Receipt file is null.");
                }

                _expenseService.TInsert(expense);

                return RedirectToAction("MyExpenses");

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ExpenseFlow_log_hata Employee/NewAddExpense : {ex.Message} ");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


