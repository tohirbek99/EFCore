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
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 10;
            var products = context.Products.OrderByDescending(x => x.ProductId)
                                                           .Include(x => x.Category)
                                                           .Skip((p - 1) * pageSize)
                                                          .Take(pageSize);
            ViewBag.PageNumer = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);


            return View(await products.ToListAsync());
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
                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exists");
                    return View(product);
                }

                string imageName = "images.png";

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHost.WebRootPath, "images/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }
                product.Image = imageName;
                context.Add(product);
                await context.SaveChangesAsync();

                TempData["Succes"] = "The product hes been added";
                return RedirectToAction("Index");

            }

            return View(product);
        }

        // GET /admin/product/details/5
        public async Task<IActionResult> Details(int id)
        {
            Product product = await context.Products.Include(x=>x.Category).FirstOrDefaultAsync(x=>x.ProductId==id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        // GET /admin/product/delete

        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();

                TempData["Error"] = "The product hes been Delete";
            }
            else
            {
                TempData["Error"] = "The product does not exist!";
            }
            return RedirectToAction("Index");
        }



    }
}
