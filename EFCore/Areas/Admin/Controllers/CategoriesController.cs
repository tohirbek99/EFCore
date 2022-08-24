using EFCore.Data;
using EFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataContext context;
        public CategoriesController(DataContext context)
        {
            this.context = context;
        }


        //get /admin/categories
        public async Task<IActionResult> Index()
        {
           return View(await context.Categories.OrderBy(x=> x.Sorting).ToListAsync());
        }



        //get /admin/categories/create
        public IActionResult Create()
        {
            return View();
        }

        // POST /admin/category/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;
                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page Already exists.");
                    return View(category);
                }
                context.Add(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been Added!";
                return RedirectToAction("Index");
            }
            return View(category);
        }


        // GET /admin/categories/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        // POST /admin/categories/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");

                var slug = await context.Categories.Where(x => x.CategoryId != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category Already exists.");
                    return View(category);
                }
                context.Update(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been edited!";

                return RedirectToAction("Edit", new { id});
            }
            return View(category);
        }



        // GET /admin/category/delete/5

        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category does not exist!";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                TempData["Error"] = "The category hes been Deleted!";
            }
            return RedirectToAction("Index");
        }
        // POST /admin/category/reprder

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var categoryId in id)
            {
                Category category = await context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;

            }
            return Ok();
        }



    }
}
