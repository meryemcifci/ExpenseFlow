using ExpenseFlow.Business.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExpenseFlow.Entity;

public class NotificationViewComponent : ViewComponent
{
    private readonly INotificationService _notificationService;
    private readonly UserManager<AppUser> _userManager;

    public NotificationViewComponent(
        INotificationService notificationService,
        UserManager<AppUser> userManager)
    {
        _notificationService = notificationService;
        _userManager = userManager;
    }
    // Bildirimleri görüntülemek için ViewComponent metodu 
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = int.Parse(_userManager.GetUserId(HttpContext.User));

        var notifications = _notificationService.GetUnreadByUserId(userId);

        var unreadCount = _notificationService.GetUnreadCount(userId);

        ViewBag.UnreadCount = unreadCount;

        return View(notifications);
    }
}
