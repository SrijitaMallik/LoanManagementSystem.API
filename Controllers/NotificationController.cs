using LoanManagementSystem.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoanManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationController(AppDbContext context)
        {
            _context = context;
        }

        // Get my notifications
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var data = await _context.LoanNotifications
                        .Where(x => x.UserId == userId)
                        .OrderByDescending(x => x.CreatedOn)
                        .ToListAsync();

            return Ok(data);
        }

        // Mark as read
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var n = await _context.LoanNotifications.FindAsync(id);
            if (n == null) return NotFound();

            n.IsRead = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
