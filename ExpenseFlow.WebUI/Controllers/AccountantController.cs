using AutoMapper;
using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Entity;
using ExpenseFlow.WebUI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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


        public AccountantController(IExpenseService expenseService, ILogger<AccountantController> logger,IMapper mapper, IHubContext<NotificationHub> hubContext, INotificationService notificationService, UserManager<AppUser> userManager)
        {
            _expenseService = expenseService;
            _logger = logger;
            _mapper = mapper;
            _hubContext = hubContext;
            _notificationService = notificationService;
            _userManager = userManager;
        }


        public IActionResult Index()//Dashboard=>Genel Bakış
        {
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


    }
}
