using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tuluc_Dragos_Proiect.Data;
using Tuluc_Dragos_Proiect.Models;

namespace Tuluc_Dragos_Proiect.Controllers
{
    public class HammocksController : Controller
    {
        private readonly ShopContext _context;

        public HammocksController(ShopContext context)
        {
            _context = context;
        }

        // GET: Hammocks
        public async Task<IActionResult> Index(
         string sortOrder,
         string currentFilter,
         string searchString,
         int? pageNumber)
         {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var hammocks = from b in _context.Hammocks
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                hammocks = hammocks.Where(s => s.Nume.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    hammocks = hammocks.OrderByDescending(b => b.Nume);
                    break;
                case "Price":
                    hammocks = hammocks.OrderBy(b => b.Pret);
                    break;
                case "price_desc":
                    hammocks = hammocks.OrderByDescending(b => b.Pret);
                    break;
                default:
                    hammocks = hammocks.OrderBy(b => b.Nume);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Hammock>.CreateAsync(hammocks.AsNoTracking(), pageNumber ??
           1, pageSize));
        }

        // GET: Hammocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hammock = await _context.Hammocks
             .Include(s => s.Orders)
             .ThenInclude(e => e.Customer)
             .AsNoTracking()
             .FirstOrDefaultAsync(m => m.ID == id);

            if (hammock == null)
            {
                return NotFound();
            }

            return View(hammock);
        }

        // GET: Hammocks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hammocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nume,Culoare,Producator,Pret")] Hammock hammock)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(hammock);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }

            return View(hammock);
        }

        // GET: Hammocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hammock = await _context.Hammocks.FindAsync(id);
            if (hammock == null)
            {
                return NotFound();
            }
            return View(hammock);
        }

        // POST: Hammocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Hammocks.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Hammock>(
            studentToUpdate,
            "",
            s => s.Nume, s => s.Culoare, s => s.Producator, s => s.Pret))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);

        }


        // GET: Hammocks/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hammock = await _context.Hammocks
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hammock == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(hammock);
        }

        // POST: Hammocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hammock = await _context.Hammocks.FindAsync(id);
            if (hammock == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Hammocks.Remove(hammock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool HammockExists(int id)
        {
            return _context.Hammocks.Any(e => e.ID == id);
        }
    }
}
