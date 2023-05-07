using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project2.Entities;

namespace project2.Areas.ADMINS.Controllers
{
    [Area("ADMINS")]
    public class LoaiController : Controller
    {
        private readonly MyDbContext _context;

        public LoaiController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Loais);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Loai loai, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                if (Hinh != null)
                {
                    var urlFull = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", "Loai", Hinh.FileName);
                    using (var file = new FileStream(urlFull, FileMode.Create))
                    {
                        await Hinh.CopyToAsync(file);
                    }

                    loai.Hinh = Hinh.FileName;
                }

                _context.Add(loai);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var loai = _context.Loais.SingleOrDefault(lo => lo.MaLoai == id);
            if (loai == null)
            {
                return RedirectToAction("Index");
            }

            return View(loai);
        }

        [HttpPost]
        public IActionResult Edit(Loai loai, IFormFile HinhUpload)
        {
            if (ModelState.IsValid)
            {
                if (HinhUpload != null)
                {
                    var urlFull = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", "Loai", HinhUpload.FileName);
                    using (var file = new FileStream(urlFull, FileMode.Create))
                    {
                        HinhUpload.CopyTo(file);
                    }

                    loai.Hinh = HinhUpload.FileName;
                }

                _context.Update(loai);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
