using Microsoft.AspNetCore.Mvc;
using DNUResourceBooker.Data;
using DNUResourceBooker.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace DNUResourceBooker.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> List()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles.ToListAsync();
                return View(user);
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}