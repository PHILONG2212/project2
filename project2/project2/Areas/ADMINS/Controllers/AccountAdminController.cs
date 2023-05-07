using Microsoft.AspNetCore.Mvc;
using project2.Entities;

namespace project2.Areas.ADMINS.Controllers
{
    [Area("ADMINS")]
    public class AccountAdminController : Controller
    {


        private readonly MyDbContext _context;

        public AccountAdminController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var dbAdmin = _context.Admins.FirstOrDefault(a => a.Taikhoan == admin.Taikhoan && a.MatKhau == admin.MatKhau);

                if (dbAdmin != null)
                {
                    // Đăng nhập thành công, thực hiện chuyển hướng đến trang chính
                    return RedirectToAction("Index", "Loai");
                }
            }

            // Đăng nhập thất bại, hiển thị lại trang đăng nhập với thông báo lỗi
            ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng");
            return View(admin);
        }
    }
}
