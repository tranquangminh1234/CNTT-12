using Microsoft.AspNetCore.Mvc;
using DNUResourceBooker.Data;
using DNUResourceBooker.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DNUResourceBooker.Controllers
{
    public class ResourceCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ResourceCategoryController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> List()
        {
            var categories = await _context.ResourceCategories.ToListAsync();
            return View(categories);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ResourceCategory model)
        {
            if (!ModelState.IsValid) return View(model);
            if (_context.ResourceCategories.Any(c => c.Name == model.Name))
            {
                ModelState.AddModelError("", "Tên loại đã tồn tại.");
                return View(model);
            }
            _context.ResourceCategories.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.ResourceCategories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ResourceCategory model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.ResourceCategories.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.ResourceCategories.FindAsync(id);
            if (category == null) return NotFound();
            _context.ResourceCategories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}