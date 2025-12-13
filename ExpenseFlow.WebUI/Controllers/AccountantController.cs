using AutoMapper;
using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.WebUI.Controllers
{

    [Authorize(Roles = "Accountant")]
    public class AccountantController : Controller
    {

        private readonly IExpenseService _expenseService;
        public readonly ILogger<AccountantController> _logger;
        private readonly IMapper _mapper;


        public AccountantController(IExpenseService expenseService, ILogger<AccountantController> logger,IMapper mapper)
        {
            _expenseService = expenseService;
            _logger = logger;
            _mapper = mapper;

        }


        public IActionResult Index()//Dashboard=>Genel Bakış
        {
            return View();
        }

        public IActionResult ApprovedExpenses()
        {
            var approvedExpenses = _expenseService.GetApprovedExpenses();
            var dtoList = _mapper.Map<List<ApprovedExpenseDto>>(approvedExpenses);

            return View(dtoList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePaymentStatus(UpdatePaymentStatusDto dto)
        {
            await _expenseService.UpdatePaymentStatusAsync(dto.ExpenseId, dto.PaymentStatus);
            TempData["Success"] = "Ödeme durumu başarıyla güncellendi.";
            return RedirectToAction("ApprovedExpenses");
        }


    }
}
