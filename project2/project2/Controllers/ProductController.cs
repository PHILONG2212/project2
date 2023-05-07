using Microsoft.AspNetCore.Mvc;

using project2.ViewModels;
using project2.Entities;
namespace project2.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyDbContext _context;

        public ProductController(MyDbContext context)
        {
            _context = context;
        }


        /*Đây là một phương thức hành động trong bộ điều khiển trả về dạng xem của tất cả các sản phẩm trong cơ sở dữ liệu.  Nó truy vấn bảng "HangHoas" trong cơ sở dữ liệu và chiếu kết quả vào một bộ sưu tập các mô hình xem "HangHoaVM" mới.  Mỗi "HangHoaVM" chứa các thuộc tính như "MaHh" (ID sản phẩm), "TenHh" (tên sản phẩm), "GiaBan" (giá bán), "Hình" (URL ảnh sản phẩm), "Loai" (tên chủng loại sản phẩm), và "NhaCungCap" (tên nhà cung cấp sản phẩm).  Cuối cùng, nó chuyển tập hợp các đối tượng "HangHoaVM" cho khung nhìn và trả lại khung nhìn cho người dùng. */
        public IActionResult Index()
        {
            var dsHangHoa = _context.HangHoas
                .Select(p => new HangHoaVM
                {
                    MaHh = p.MaHh,
                    TenHh = p.TenHh,
                    GiaBan = p.DonGia.Value,
                    Hinh = p.Hinh,
                    Loai = p.MaLoaiNavigation.TenLoai,
                    NhaCungCap = p.MaNccNavigation.TenCongTy
                });

            return View(dsHangHoa);
        }



        /*Mã này xác định một phương thức hành động trong bộ điều khiển ASP.NET Core MVC có tên "Chi tiết" trả về dạng xem cho một trang chi tiết sản phẩm cụ thể dựa trên ID sản phẩm đã cho. 

Phương thức nhận một tham số nguyên idđại diện cho ID của sản phẩm sẽ hiển thị.  Sau đó nó truy vấn các _contextphiên bản của cơ sở dữ liệu để truy xuất sản phẩm với ID đã cho bằng cách sử dụng SingleOrDefaultphương pháp. 

Nếu tìm thấy một sản phẩm có ID đã cho, phương thức này sẽ trả về một dạng xem để hiển thị thông tin chi tiết của sản phẩm đó bằng cách sử dụng View(item)phương pháp, ở đâu itemlà sản phẩm thu hồi.  Nếu không tìm thấy sản phẩm có ID đã cho, phương thức này sẽ chuyển hướng người dùng đến hành động Chỉ mục của cùng một bộ điều khiển bằng cách sử dụng RedirectToAction("Index"). */
        public IActionResult Detail(int id)
        {
            var item = _context.HangHoas.SingleOrDefault(hh => hh.MaHh == id);
            if (item != null)
            {
                return View(item);
            }

            return RedirectToAction("Index");
        }
    }
}
