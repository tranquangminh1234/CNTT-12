using Microsoft.AspNetCore.Mvc;
using DNUResourceBooker.Data;
using DNUResourceBooker.Models;
using DNUResourceBooker.Models.ViewModels.Booking;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace DNUResourceBooker.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Create(int resourceId)
        {
            var resource = await _context.Resources.FindAsync(resourceId);
            if (resource == null || !resource.IsAvailable) return NotFound();
            var model = new BookingCreateViewModel
            {
                ResourceId = resourceId,
                ResourceName = resource.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (userId == 0) return RedirectToAction("Login", "Account");

            var start = model.BookingDate.Date + model.StartTime;
            var end = model.BookingDate.Date + model.EndTime;

            var conflict = await _context.Bookings.AnyAsync(b =>
                b.ResourceId == model.ResourceId &&
                b.BookingStatus == "Approved" &&
                start < b.EndTime && end > b.StartTime);

            if (conflict)
            {
                ModelState.AddModelError("", "Khung giờ này đã có người đặt.");
                return View(model);
            }

            var booking = new Booking
            {
                ResourceId = model.ResourceId,
                UserId = userId,
                StartTime = start,
                EndTime = end,
                Purpose = model.Purpose,
                BookingStatus = "Pending",
                CreatedAt = DateTime.Now
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var bookings = await _context.Bookings
                .Include(b => b.Resource)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return View(bookings);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (booking == null || booking.UserId != userId) return NotFound();
            if ((booking.StartTime - DateTime.Now).TotalHours < 24)
            {
                TempData["Error"] = "Bạn chỉ được hủy trước 24h.";
                return RedirectToAction("List");
            }
            booking.BookingStatus = "CancelledByUser";
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}