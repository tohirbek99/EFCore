using EFCore.Data;
using EFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly DataContext context;
        private readonly IWebHostEnvironment webHost;

        public ProductsController(DataContext Context, IWebHostEnvironment webHost)
        {
           this.context = Context;
            this.webHost = webHost;
        }


        // GET /admin/product
        public async  Task<IActionResult> Index()
        {
            return View(await context.Products.OrderByDescending(x=>x.ProductId).Include(x=>x.Category).ToListAsync());
        }


        // GET /admin/product/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "CategoryId", "Name");


            return View();
        }
        // POST /admin/product/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "CategoryId", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                var slug= await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug!=null)
                {
                    ModelState.AddModelError("", "The product already exists");
                    return View(product);
                }

                string imageNames = "Noimage.png";
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHost.WebRootPath, "images/image");
                    imageNames = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath=Path.Combine(uploadsDir, imageNames);
                    FileStream fs=new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }
                product.Image = imageNames;
                context.Add(product);
                await context.SaveChangesAsync();

                TempData["Succes"] = "The product hes been added";
                return RedirectToAction("Index");

            }

            return View(product);
        }
    }
}
