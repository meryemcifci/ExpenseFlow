using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.WebUI.Controllers
{

    [Authorize(Roles = "Manager")]
    public class ExpenseApprovalController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpenseApprovalController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        
        public IActionResult Index()
        {

            //employee talebi girdikten sonra bekleyen stauslu expenseleri görür.
            var pendingExpenses = _expenseService.GetPendingWithCategory();
            ViewBag.PendingCount = pendingExpenses.Count;

            return View(pendingExpenses);
        }

        
        //public IActionResult Approve(int id)
        //{
        //    var expense = _expenseService.TGetById(id);
        //    if (expense == null)
        //        return NotFound();

        //    expense.Status = ExpenseStatus.Approved;

        //    _expenseService.TUpdate(expense);

        //    TempData["SuccessMessage"] = "Masraf talebi onaylandı.";
        //    return RedirectToAction("Index");
        //}

       
       
        //public IActionResult Reject(int id)
        //{
        //    var expense = _expenseService.TGetById(id);
        //    if (expense == null)
        //        return NotFound();

        //    expense.Status = ExpenseStatus.Rejected;

        //    _expenseService.TUpdate(expense);

        //    TempData["ErrorMessage"] = "Masraf talebi reddedildi.";
        //    return RedirectToAction("Index");
        //}

        public IActionResult ApprovedExpenses()
        {
            var expenses = _expenseService
                .TGetList()
                .Where(x => x.Status == ExpenseStatus.Approved)
                .ToList();

            return View(expenses);
        }

        public IActionResult RejectedExpenses()
        {
            var expenses = _expenseService
                .TGetList()
                .Where(x => x.Status == ExpenseStatus.Rejected)
                .ToList();

            return View(expenses);
        }

    }
}
