using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;
[ApiController]
[Route("api/[controller]")]
public class NotificationController(INotificationService _notificationService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<NotificationDto>>> GetNotifications()
    {
        return Ok(_notificationService.GetPurchaseOrderPending());
    }
}