using Microsoft.AspNetCore.Mvc;
using project2.Entities;

namespace project2.Areas.ADMINS.Controllers
{
    [Area("ADMINS")]
    public class NhaCungCapController : Controller
    {
        
        private readonly MyDbContext _context;

        public NhaCungCapController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.NhaCungCaps);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhaCungCap nhaCungCap, IFormFile Hinh)
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

                    nhaCungCap.Logo = Hinh.FileName;
                }

                _context.Add(nhaCungCap);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(string id)
        {
            var loai = _context.NhaCungCaps.SingleOrDefault(lo => lo.MaNcc == id);
            if (loai == null)
            {
                return RedirectToAction("Index");
            }

            return View(loai);
        }

        [HttpPost]
        public IActionResult Edit(NhaCungCap loai, IFormFile HinhUpload)
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

                    loai.Logo = HinhUpload.FileName;
                }

                _context.Update(loai);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
