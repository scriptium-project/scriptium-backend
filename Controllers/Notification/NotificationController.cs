using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.NotificationHandler
{

    [ApiController, Route("notification"), Authorize]
    public class NotificationController(ApplicationDBContext db, UserManager<User> userManager) : ControllerBase
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        [HttpGet, Route("{quantity?}")]
        public async Task<IActionResult> GetNotifications([FromRoute] int? quantity)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            List<Notification> data;

            IQueryable<Notification> notificationsQuery = _db.Notification
                .Where(n => n.RecipientId == UserRequested.Id)
                .OrderByDescending(n => n.CreatedAt);

            if (quantity.HasValue)
                notificationsQuery = notificationsQuery.Take(quantity.Value);

            data = await notificationsQuery.ToListAsync();

            return Ok(new { data });
        }

        [HttpPut, Route("read")]
        public async Task<IActionResult> GetNotifications([FromBody] HashSet<long> ReadNotificationsIds)
        {
            string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null)
                return Unauthorized();

            User? UserRequested = await _userManager.FindByIdAsync(UserId);

            if (UserRequested == null)
                return NotFound(new { Message = "User not found." });

            List<Notification> Notifications = await _db.Notification.Where(n => n.RecipientId == UserRequested.Id && ReadNotificationsIds.Contains(n.Id)).ToListAsync();

            foreach (Notification notification in Notifications)
                notification.IsRead = true;


            try
            {
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Successfully have read!" });

            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went unexpectedly wrong?" });
            }

        }
    }
}