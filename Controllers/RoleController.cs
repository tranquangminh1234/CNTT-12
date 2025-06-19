using Microsoft.AspNetCore.Mvc;
using DNUResourceBooker.Data;
using DNUResourceBooker.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DNUResourceBooker.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RoleController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> List()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Role model)
        {
            if (!ModelState.IsValid) return View(model);
            if (_context.Roles.Any(r => r.RoleName == model.RoleName))
            {
                ModelState.AddModelError("", "Tên quyền đã tồn tại.");
                return View(model);
            }
            _context.Roles.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Role model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.Roles.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}