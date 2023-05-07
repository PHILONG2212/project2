using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using project2.Entities;
using project2.ViewModels;

namespace project2.Controllers
{
    public class HangHoaController : Controller
    {
        //một đối tượng DbContext (được đặt tên là MyDbContext) để truy cập cơ sở dữ liệu.
        private readonly MyDbContext _context;

        public HangHoaController(MyDbContext context)
        {
            _context = context;
        }


       /* Đoạn mã này là một phương thức của một controller trong một ứng dụng ASP.NET Core.Phương thức này được sử dụng để hiển thị danh sách sản phẩm trên một trang web, mỗi trang hiển thị tối đa 6 sản phẩm. Các bước thực hiện như sau:

    Định nghĩa một hằng số SO_PHAN_TU_MOI_TRANG với giá trị 6, là số lượng sản phẩm hiển thị trên một trang. 

    Phương thức Index có một tham số là page, mặc định có giá trị là 1, đại diện cho số trang cần hiển thị. 

    Truy vấn cơ sở dữ liệu để lấy ra một danh sách các sản phẩm (HangHoa) thông qua đối tượng _context. Sau đó, sử dụng phương thức Skip() để bỏ qua số lượng sản phẩm trước trang cần hiển thị và Take() để lấy số lượng sản phẩm tối đa cần hiển thị trên trang đó.Việc này giúp cho ứng dụng có thể phân trang.

    Sử dụng phương thức Select() để chọn các trường dữ liệu cần hiển thị từ danh sách sản phẩm, và đưa các trường dữ liệu này vào một đối tượng ViewModel tương ứng (HangHoaVM) để trả về cho View.

    Trả về một View với đối số là danh sách các sản phẩm được chọn (HangHoaVM) ở bước trước.

Sau khi hoàn thành các bước trên, View sẽ hiển thị danh sách các sản phẩm trên một trang web và người dùng có thể sử dụng các phương thức phân trang để xem các sản phẩm khác.
*/


        private int SO_PHAN_TU_MOI_TRANG = 6;
        public IActionResult Index(int page = 1)
        {
            var dsHangHoa = _context.HangHoas
                .Skip((page - 1) * SO_PHAN_TU_MOI_TRANG)
                .Take(SO_PHAN_TU_MOI_TRANG)
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

        public IActionResult TimKiem()
        {
            return View();
        }
       

        // sử dụng kiểu ajax
        [HttpPost]
        public IActionResult TimAjax(string Keyword)
        {
            var dsHangHoa = _context.HangHoas.AsQueryable();

            if (!string.IsNullOrEmpty(Keyword))
            {
                dsHangHoa = dsHangHoa.Where(hh => hh.TenHh.Contains(Keyword));
            }

            var data = dsHangHoa.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                GiaBan = p.DonGia.Value,
                Hinh = p.Hinh,
                Loai = p.MaLoaiNavigation.TenLoai,
                NhaCungCap = p.MaNccNavigation.TenCongTy
            });

            return PartialView(data);
        }




        /* Đoạn mã trên thực hiện tính toán doanh thu theo hàng hóa và trả về kết quả dưới dạng một View.

Cụ thể, đoạn mã truy vấn các chi tiết hóa đơn từ bảng ChiTietHds trong cơ sở dữ liệu sử dụng đối tượng MyDbContext. Thông qua phương thức Include(), đoạn mã cũng kết hợp các chi tiết hóa đơn với bảng HangHoas để có thể truy cập thông tin về tên và giá của hàng hóa tương ứng.

Sau đó, đoạn mã nhóm các chi tiết hóa đơn theo mã hàng hóa (MaHh), tính tổng số lượng và doanh thu cho mỗi mã hàng hóa bằng cách sử dụng phương thức Sum(). Giá bán thực tế được tính bằng công thức (DonGia * (1 - GiamGia)), trong đó DonGia và GiamGia lần lượt là giá bán và tỷ lệ giảm giá của mỗi chi tiết hóa đơn.

Cuối cùng, danh sách các mặt hàng được sắp xếp giảm dần theo doanh thu và được truyền vào một View để hiển thị kết quả lên giao diện người dùng.*/

        public IActionResult DoanhThuTheoHangHoa()
        {
            var doanhThuTheoHangHoa = _context.ChiTietHds
                .Include(cthd => cthd.MaHhNavigation)
                .GroupBy(cthd => cthd.MaHh)
                .Select(grp => new
                {
                    MaHh = grp.Key,
                    TenHh = grp.First().MaHhNavigation.TenHh,
                    SoLuong = grp.Sum(cthd => cthd.SoLuong),
                    DoanhThu = grp.Sum(cthd => cthd.SoLuong * cthd.DonGia * (1 - cthd.GiamGia))
                })
                .OrderByDescending(x => x.DoanhThu)
                .ToList();

            return View(doanhThuTheoHangHoa);
        }







        /*Đây là một phương thức trong bộ điều khiển ASP.NET Core truy xuất doanh thu theo sản phẩm và tháng từ cơ sở dữ liệu và trả về dạng xem hiển thị dữ liệu.  Dưới đây là bảng phân tích từng bước về chức năng của mã: 

    Phương thức bắt đầu bằng việc định nghĩa một biến doanhThuHangThangsẽ chứa dữ liệu về doanh thu theo sản phẩm và tháng. 
    Các Includephương pháp được sử dụng để háo hức tải MaHhNavigationthuộc tính điều hướng của ChiTietHdsthực thể để tránh các vấn đề về hiệu suất tải chậm. 
    Các Wherephương pháp được sử dụng để lọc các ChiTietHdsthực thể theo năm để chỉ các giao dịch xảy ra trong năm hiện tại được xem xét. 
    Các GroupByphương pháp được sử dụng để nhóm các giao dịch theo cả sản phẩm và tháng. 
    Các Selectđược sử dụng để chiếu các giao dịch được nhóm thành một loại ẩn danh mới có chứa ID sản phẩm, tên sản phẩm, tháng, tổng số lượng đã bán và tổng doanh thu được tạo. 
    Các OrderByDescendingphương pháp được sử dụng để sắp xếp tập hợp kết quả theo doanh thu theo thứ tự giảm dần. 
    Tập kết quả được cụ thể hóa thành một danh sách bằng cách gọi hàm ToListphương pháp. 
    Các doanhThuHangThangbiến được truyền cho Viewphương thức trả về dạng xem hiển thị dữ liệu. 

Nhìn chung, phương pháp này truy xuất doanh thu do từng sản phẩm tạo ra trong mỗi tháng của năm hiện tại và sắp xếp kết quả được đặt theo doanh thu theo thứ tự giảm dần.  */
        public IActionResult DoanhThuHangThang()
        {
            var doanhThuHangThang = _context.ChiTietHds
                .Include(cthd => cthd.MaHhNavigation)
                .Where(cthd => cthd.MaHhNavigation.NgaySx.Year == DateTime.Now.Year) // chỉ tính trong năm hiện tại
                .GroupBy(cthd => new
                {
                    cthd.MaHh,
                    Thang = cthd.MaHhNavigation.NgaySx.Month
                })
                .Select(grp => new
                {
                    MaHh = grp.Key.MaHh,
                    TenHh = grp.First().MaHhNavigation.TenHh,
                    Thang = grp.Key.Thang,
                    SoLuong = grp.Sum(cthd => cthd.SoLuong),
                    DoanhThu = grp.Sum(cthd => cthd.SoLuong * cthd.DonGia * (1 - cthd.GiamGia))
                })
                .OrderByDescending(x => x.DoanhThu)
                .ToList();

            return View(doanhThuHangThang);
        }
    }
}
