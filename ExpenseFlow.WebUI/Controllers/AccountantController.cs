using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.Services;
using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Entity;
using ExpenseFlow.WebUI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ExpenseFlow.WebUI.Controllers
{

    [Authorize(Roles = "Accountant")]
    public class AccountantController : Controller
    {

        private readonly IExpenseService _expenseService;
        public readonly ILogger<AccountantController> _logger;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        private IReportService _reportService;
        private readonly IAccountantService _accountantService;


        public AccountantController(IExpenseService expenseService, ILogger<AccountantController> logger,IMapper mapper, IHubContext<NotificationHub> hubContext, INotificationService notificationService, UserManager<AppUser> userManager, IReportService reportService, IAccountantService accountantService)
        {
            _expenseService = expenseService;
            _logger = logger;
            _mapper = mapper;
            _hubContext = hubContext;
            _notificationService = notificationService;
            _userManager = userManager;
            _reportService = reportService;
            _accountantService = accountantService;
        }


        public async Task<IActionResult> Index()
        {

            var approvedExpenses = _expenseService.GetApprovedExpenses();
            var paidExpenses = _expenseService.GetPaidExpenses();

            ViewBag.TotalApproved = approvedExpenses.Sum(x => x.Amount);
            ViewBag.TotalPaid = paidExpenses.Sum(x => x.Amount);

            ViewBag.TotalPending = approvedExpenses
                .Where(x => x.PaymentStatus == PaymentStatus.Pending)
                .Sum(x => x.Amount);

            ViewBag.ApprovedCount = approvedExpenses.Count;

            var departmentCounts =
                await _expenseService.GetApprovedExpenseCountByDepartmentAsync();

            var categoryCounts =
                await _expenseService.GetExpenseCountsByCategoryAsync();

            ViewBag.DepartmentLabels = departmentCounts.Keys.ToList();
            ViewBag.DepartmentData = departmentCounts.Values.ToList();

            ViewBag.CategoryLabels = categoryCounts.Keys.ToList();
            ViewBag.CategoryData = categoryCounts.Values.ToList();

            return View();
        }

        public IActionResult ApprovedExpenses()
        {
            var approvedExpenses = _expenseService.GetApprovedExpenses();
            var dtoList = _mapper.Map<List<ApprovedExpenseDto>>(approvedExpenses);
            var userId = int.Parse(_userManager.GetUserId(User));
            //Zil Count
            ViewBag.UnreadCount = _notificationService.GetUnreadCount(userId);
            // Dropdown
            ViewBag.Notifications = _notificationService.GetByUserId(userId);
            return View(dtoList);
        }


        [HttpPost]
        public async Task<IActionResult> ApproveExpense(int id)
        {
            // Masrafı onayla (örnek: durumunu değiştir)
            var expense = _expenseService.TGetById(id);
            if (expense == null)
            {
                return NotFound();
            }

            expense.Status = ExpenseStatus.Approved;
            _expenseService.TUpdate(expense);

            // Bildirim gönder (Accountant grubuna)
            await _notificationService.CreateNotificationForUserAsync(
                id,
                $"{User.Identity.Name} bir masrafı onayladı.",
                "/Accountant/ApprovedExpenses"
            );


            //var userId = User.GetUserIdInt(); // extension method yazarak kullanabilirdik.
            //ViewBag.UnreadCount = _notificationService.GetUnreadCount(userId);

            var userId = int.Parse(_userManager.GetUserId(User));
            ViewBag.UnreadCount = _notificationService.GetUnreadCount(userId);



            // Tekrar liste sayfasına yönlendir
            return RedirectToAction("ApprovedExpenses");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePaymentStatus(UpdatePaymentStatusDto dto)
        {
            await _expenseService.UpdatePaymentStatusAsync(dto.ExpenseId, dto.PaymentStatus);

            //  Admin’e
            await _notificationService.CreateNotificationForRoleAsync(
                "Admin",
                "Bir masraf ödemesi tamamlandı.",
                "/Admin/Index"
            );

            if (dto.PaymentStatus == PaymentStatus.Paid)
            {
                var expense = _expenseService.TGetById(dto.ExpenseId);

                await _notificationService.CreateNotificationForUserAsync(
                    expense.UserId,
                    "Masrafınız muhasebe tarafından ödendi.",
                    "/Employee/MyExpenses"
                );
            }


            TempData["Success"] = "Ödeme durumu başarıyla güncellendi.";


           

            return RedirectToAction("ApprovedExpenses");
        }


        [HttpGet]
        public IActionResult PaidExpenses()
        {
            var paidExpenses = _expenseService.GetPaidExpenses();
            var dtoList = _mapper.Map<List<ApprovedExpenseDto>>(paidExpenses);

            var userId = int.Parse(_userManager.GetUserId(User));
            ViewBag.UnreadCount = _notificationService.GetUnreadCount(userId);
            ViewBag.Notifications = _notificationService.GetByUserId(userId);

            return View(dtoList);
        }

        public IActionResult MonthlyReport()
        {
           

            return View();
        }
        // PDF İNDİRME
        [Authorize(Roles = "Accountant")]
        [HttpGet]
        public async Task<IActionResult> DownloadMonthlyAccountantPdf(
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
                .GenerateMonthlyAccountantPdfAsync(year, month, logoPath);

            var fileName =
                $"AccountantExpenseReport_{year}_{month:D2}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

            return File(
                pdfBytes,
                "application/pdf",
                fileName
            );
        }

        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var profile = await _accountantService.GetAccountantProfileAsync(userId);

            return View(profile);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            var userId = int.Parse(
               User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );
            await _accountantService.UpdateProfile(userId, model);
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

            await _accountantService.UpdateProfilePhotoAsync(userId, photoUrl);

            return RedirectToAction("Profile");
        }





    }
}
