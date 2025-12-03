using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ExpenseFlow.WebUI.Controllers
{

    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult MANAGER()
        {
            return View();
        }


    }

}
