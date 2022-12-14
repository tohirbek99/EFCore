using EFCore.Data;
using EFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PageController : Controller
    {
        private readonly DataContext context;
        public PageController(DataContext context)
        {
            this.context = context;
        }
            

        // GET /admin/pages
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in context.Pages orderby p.Sorting select p;
            List<Page> Plist = await pages.ToListAsync();
            ViewBag.Fruit = "Apples";
            return View(Plist);
        }

        // GET /admin/pages/details/5
        public async Task<IActionResult> Details(int id)
        {
            Page page = await context.Pages.FirstOrDefaultAsync(x => x.PageId == id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // GET /admin/page/create/5
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // POST /admin/page/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;
                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page Already exists.");
                    return View(page);
                }
                context.Add(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The page has been Added!";
                return RedirectToAction("Index");
            }

            return View(page);
        }

        // GET /admin/page/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }


        // POST /admin/page/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.PageId == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");

                var slug = await context.Pages.Where(x => x.PageId != page.PageId).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page Already exists.");
                    return View(page);
                }
                context.Update(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "The page has been edited!";

                return RedirectToAction("Edit", new { id = page.PageId });
            }
            return View(page);
        }

        // GET /admin/page/delete/5

        public async Task<IActionResult> Delete(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "The page does not exist!";
            }
            else
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();

                TempData["Error"] = "The page hes been Deleted!";
            }
            return RedirectToAction("Index");
        }


        // POST /admin/page/reprder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var pageId in id)
            {
                Page page = await context.Pages.FindAsync(pageId);
                page.Sorting=count;
                context.Update(page);
                await context.SaveChangesAsync();
                count++;

            }
            return Ok();
        }
    }
}
