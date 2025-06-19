using Microsoft.AspNetCore.Mvc;
using DNUResourceBooker.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DNUResourceBooker.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings.Include(b => b.Resource).Include(b => b.User).OrderByDescending(b => b.CreatedAt).ToListAsync();
            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null || booking.BookingStatus != "Pending") return NotFound();

            var conflict = await _context.Bookings.AnyAsync(b =>
                b.ResourceId == booking.ResourceId &&
                b.BookingStatus == "Approved" &&
                booking.StartTime < b.EndTime && booking.EndTime > b.StartTime
            );
            if (conflict)
            {
                TempData["Error"] = "Khung giờ đã có người đặt.";
                return RedirectToAction("Bookings");
            }

            booking.BookingStatus = "Approved";
            await _context.SaveChangesAsync();
            return RedirectToAction("Bookings");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null || booking.BookingStatus != "Pending") return NotFound();
            booking.BookingStatus = "CancelledByAdmin";
            await _context.SaveChangesAsync();
            return RedirectToAction("Bookings");
        }
    }
}