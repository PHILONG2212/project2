using Microsoft.AspNetCore.Mvc;
using project2.Models;
using System.Diagnostics;

namespace project2.Controllers
{
    /*Đây là lớp bộ điều khiển ASP.NET Core điển hình với ba phương thức hành động.  Hàm tạo đưa vào một thể hiện của ILogger<HomeController>để kích hoạt ghi nhật ký. 

Các Indexphương thức hành động trả về mặc định Indexquan điểm của HomeController. 

Các Privacyphương thức hành động trả về Privacyquan điểm của HomeController. 

Các Errorphương pháp hành động được trang trí với [ResponseCache]thuộc tính, vô hiệu hóa bộ nhớ đệm của phản hồi bằng cách đặt Durationthuộc tính thành 0 và Locationtài sản để ResponseCacheLocation.None.  Phương thức này trả về Errorxem với một ErrorViewModelđối tượng bao gồm một RequestIdthuộc tính để xác định yêu cầu trong trường hợp có lỗi. */
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}