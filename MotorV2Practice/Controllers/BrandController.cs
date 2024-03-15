using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorV2Practice.Data;
using MotorV2Practice.Models;

namespace MotorV2Practice.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(ApplicationDBContext dBContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dBContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.Brands.ToList();
            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Create(Brand brand)  
        {
            string webrootpath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;
            
            if(file.Count > 0)
            {
                string newfilename = Guid.NewGuid().ToString();

                var upload = Path.Combine(webrootpath, @"Images/Brand");

                var extension = Path.GetExtension(file[0].FileName);

                using (var filestream = new FileStream(Path.Combine(upload, newfilename + extension), FileMode.Create)) 
                {
                    file[0].CopyTo(filestream);
                }

                brand.BrandLogo = @"\Images\Brand\" + newfilename + extension;
            }
            if(ModelState.IsValid)
            {
                _dbContext.Brands.Add(brand);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpGet]
        public IActionResult Edit(Guid id) 
        {
            Brand brand = _dbContext.Brands.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            string webrootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if(file.Count > 0) 
            {
                var newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webrootPath, @"Images/Brand");

                var extension = Path.GetExtension(file[0].FileName);

                //Delete old Objects
                var objFromDb = _dbContext.Brands.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if(objFromDb.BrandLogo != null) 
                {
                    var oldImagePath = Path.Combine(webrootPath,objFromDb.BrandLogo.Trim('\\'));
                    if(System.IO.File.Exists(oldImagePath)) 
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.BrandLogo = @"\Images\Brand\" + newFileName + extension;
            }

            if(ModelState.IsValid) 
            {
                var objFromDb = _dbContext.Brands.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                objFromDb.Name = brand.Name;
                objFromDb.EstablisgYear = brand.EstablisgYear;

                if (brand.BrandLogo != null)
                {
                    objFromDb.BrandLogo = brand.BrandLogo;
                }

                _dbContext.Brands.Update(objFromDb);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpPost]
        public IActionResult Delete(Brand brand) 
        {
            string webrootPath = _webHostEnvironment.WebRootPath;

            if(string.IsNullOrEmpty(brand.BrandLogo)) 
            {
                //Delete old Objects
                var objFromDb = _dbContext.Brands.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if (objFromDb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webrootPath, objFromDb.BrandLogo.Trim('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
            }
            _dbContext.Brands.Remove(brand);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
