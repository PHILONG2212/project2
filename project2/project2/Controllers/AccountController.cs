using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project2.Entities;

namespace project2.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;

        public AccountController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Register(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.KhachHangs.Add(khachHang);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(khachHang);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var khachHang = _context.KhachHangs.FirstOrDefault(kh => kh.Email == email && kh.MatKhau == password);
            if (khachHang != null)
            {
                // Đăng nhập thành công, lưu thông tin vào session và chuyển hướng đến trang chủ
                HttpContext.Session.SetInt32("MaKh", khachHang.MaKh);
                HttpContext.Session.SetString("HoTen", khachHang.HoTen);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Đăng nhập không thành công, hiển thị thông báo lỗi
                ModelState.AddModelError("", "Đăng nhập không thành công");
                return View();
            }
        }

        public IActionResult Logout()
        {
            // Xóa thông tin khách hàng trong session và chuyển hướng đến trang chủ
            HttpContext.Session.Remove("MaKh");
            HttpContext.Session.Remove("HoTen");
            return RedirectToAction("Index", "Home");
        }
    }
}
