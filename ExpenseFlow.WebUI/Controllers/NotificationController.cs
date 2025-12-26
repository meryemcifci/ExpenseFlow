using ExpenseFlow.Business.Abstract;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.WebUI.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public IActionResult ReadAndRedirect([FromBody] ReadNotificationDto dto)
        {
            _notificationService.MarkAsRead(dto.NotificationId);
            return Ok();
        }
    }
}
