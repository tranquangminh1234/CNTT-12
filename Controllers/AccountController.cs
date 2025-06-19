using Microsoft.AspNetCore.Mvc;
using DNUResourceBooker.Models.ViewModels.Account;
using DNUResourceBooker.Data;
using DNUResourceBooker.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DNUResourceBooker.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            string hash = HashPassword(model.Password);
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == hash);
            if (user == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Role", user.RoleId == 1 ? "Admin" : "User");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "Email đã được đăng ký");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                RoleId = 2 // User
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return string.Concat(bytes.Select(x => x.ToString("x2")));
        }

        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email không tồn tại");
                return View();
            }
            TempData["Message"] = "Đã gửi email hướng dẫn đổi mật khẩu (giả lập)";
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword() => View();

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var user = _context.Users.Find(userId);
            if (user == null) return RedirectToAction("Login");
            if (user.PasswordHash != HashPassword(oldPassword))
            {
                ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                return View();
            }
            user.PasswordHash = HashPassword(newPassword);
            _context.SaveChanges();
            TempData["Message"] = "Đổi mật khẩu thành công";
            return RedirectToAction("Login");
        }
    }
}