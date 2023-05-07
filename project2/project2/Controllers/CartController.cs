using Microsoft.AspNetCore.Mvc;

using project2.Helpers;
using project2.Models;
using project2.Entities;
namespace project2.Controllers
{
  
    public class CartController : Controller
    {
        private readonly MyDbContext _context;

        public CartController(MyDbContext context)
        {
            _context = context;
        }


        /*Đây là thuộc tính trả về danh sách CartItemcác đối tượng đại diện cho trạng thái hiện tại của giỏ hàng.  Nó lấy dữ liệu giỏ hàng từ HttpContext.Sessionđối tượng bằng cách gọi Getphương thức mở rộng và chuyển tên của khóa phiên làm tham số.  Nếu khóa phiên không tồn tại hoặc giá trị được lưu trữ trong phiên là null, nó sẽ tạo một danh sách mới gồm CartItemcác đối tượng và trả về nó. */
        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }

        public IActionResult Index()
        {
            return View(Carts);
        }

        /*Đoạn mã này định nghĩa một phương thức hành động có tên AddToCartbên trong CartControllerlớp học.  Nó nhận hai tham số: id, là một số nguyên đại diện cho ID sản phẩm và SoLuong, là số nguyên thể hiện số lượng sản phẩm cần thêm vào giỏ hàng. 

Đầu tiên, phương thức lấy các mặt hàng trong giỏ hàng hiện tại từ phiên bằng cách sử dụng Cartsthuộc tính, lấy ra một danh sách các CartItemcác đối tượng.  Sau đó, nó sẽ kiểm tra xem sản phẩm có ID đã cho đã tồn tại trong giỏ hàng chưa bằng cách gọi SingleOrDefaultphương pháp trên myCartdanh sách, chuyển vào một biểu thức lambda so sánh MaHhtài sản của mỗi người CartItemvới đã cho id. 

Nếu không tìm thấy sản phẩm trong giỏ hàng, phương thức sẽ tìm nạp chi tiết sản phẩm từ cơ sở dữ liệu bằng cách sử dụng đã cho idvà tạo ra một cái mới CartItemđối tượng với các chi tiết đã tìm nạp và đã cho SoLuong.  Sau đó, nó thêm cái mới CartItemphản đối myCartdanh sách. 

Nếu sản phẩm đã có trong giỏ hàng, phương thức này chỉ cần tăng SoLuongtài sản của CartItemđối tượng bởi đã cho SoLuong. 

Cuối cùng, phương pháp lưu trữ cập nhật myCartliệt kê trong phiên bằng cách sử dụng HttpContext.Session.Setphương thức, chuyển vào myCartdanh sách và một khóa chuỗi "GioHang".  Sau đó, nó chuyển hướng người dùng đến Indexhành động của CartController. */
        public IActionResult AddToCart(int id, int SoLuong)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.MaHh == id);

            if (item == null)//chưa có
            {
                var hangHoa = _context.HangHoas.SingleOrDefault(p => p.MaHh == id);
                item = new CartItem
                {
                    MaHh = id,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia.Value,
                    SoLuong = SoLuong,
                    Hinh = hangHoa.Hinh
                };
                myCart.Add(item);
            }
            else
            {
                item.SoLuong += SoLuong;
            }
            HttpContext.Session.Set("GioHang", myCart);

            return RedirectToAction("Index");
        }
    }

}
