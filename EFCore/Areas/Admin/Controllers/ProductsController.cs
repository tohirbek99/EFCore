using EFCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly DataContext context;

        public ProductsController(DataContext Context)
        {
           this.context = Context;
        }


        // GET /admin/product
        public async  Task<IActionResult> Index()
        {
            return View(await context.Products.OrderByDescending(x=>x.ProductId).Include(x=>x.Category).ToListAsync());
        }


        // GET /admin/product/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "CategoryId", "Name");


            return View();
        }
    }
}
