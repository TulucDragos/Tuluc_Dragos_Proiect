using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tuluc_Dragos_Proiect.Data;
using Tuluc_Dragos_Proiect.Models;
using Tuluc_Dragos_Proiect.Models.LibraryViewModels;

namespace Tuluc_Dragos_Proiect.Controllers
{
    public class DistribuitorsController : Controller
    {
        private readonly ShopContext _context;

        public DistribuitorsController(ShopContext context)
        {
            _context = context;
        }

        // GET: Distribuitors
        public async Task<IActionResult> Index(int? id, int? hammockID)
        {
            var viewModel = new DistribuitorIndexData();
            viewModel.Distribuitors = await _context.Distribuitors
            .Include(i => i.DistributedHammocks)
            .ThenInclude(i => i.Hammock)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.DistributorName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["PublisherID"] = id.Value;
                Distribuitor distribuitor = viewModel.Distribuitors.Where(
                i => i.ID == id.Value).Single();
                viewModel.Hammocks = distribuitor.DistributedHammocks.Select(s => s.Hammock);
            }
            if (hammockID != null)
            {
                ViewData["HammockID"] = hammockID.Value;
                viewModel.Orders = viewModel.Hammocks.Where(
                x => x.ID == hammockID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Distribuitors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribuitor = await _context.Distribuitors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distribuitor == null)
            {
                return NotFound();
            }

            return View(distribuitor);
        }

        // GET: Distribuitors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distribuitors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DistributorName,Adress")] Distribuitor distribuitor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distribuitor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distribuitor);
        }

        // GET: Distribuitors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var distribuitor = await _context.Distribuitors
            .Include(i => i.DistributedHammocks).ThenInclude(i => i.Hammock)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (distribuitor == null)
            {
                return NotFound();
            }
            PopulateDistributedHammockData(distribuitor);
            return View(distribuitor);

        }
        private void PopulateDistributedHammockData(Distribuitor distribuitor)
        {
            var allHammocks = _context.Hammocks;
            var distributedHammocks = new HashSet<int>(distribuitor.DistributedHammocks.Select(c => c.HammockID));
            var viewModel = new List<DistributedHammockData>();
            foreach (var hammock in allHammocks)
            {
                viewModel.Add(new DistributedHammockData
                {
                    HammockID = hammock.ID,
                    Title = hammock.Nume,
                    IsDistributed = distributedHammocks.Contains(hammock.ID)
                });
            }
            ViewData["Hammocks"] = viewModel;
        }

        // POST: Distribuitors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] distributedHammocks)
        {
            if (id == null)
            {
                return NotFound();
            }
            var distribuitorToUpdate = await _context.Distribuitors
            .Include(i => i.DistributedHammocks)
            .ThenInclude(i => i.Hammock)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Distribuitor>(
            distribuitorToUpdate,
            "",
            i => i.DistributorName, i => i.Adress))
            {
                UpdatePublishedBooks(distributedHammocks, distribuitorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdatePublishedBooks(distributedHammocks, distribuitorToUpdate);
            PopulateDistributedHammockData(distribuitorToUpdate);
            return View(distribuitorToUpdate);
        }
        private void UpdatePublishedBooks(string[] selectedHammocks, Distribuitor distribuitorToUpdate)
        {
            if (selectedHammocks == null)
            {
                distribuitorToUpdate.DistributedHammocks = new List<DistributedHammock>();
                return;
            }
            var selectedHammocksHS = new HashSet<string>(selectedHammocks);
            var publishedBooks = new HashSet<int>
            (distribuitorToUpdate.DistributedHammocks.Select(c => c.Hammock.ID));
            foreach (var hammock in _context.Hammocks)
            {
                if (selectedHammocksHS.Contains(hammock.ID.ToString()))
                {
                    if (!publishedBooks.Contains(hammock.ID))
                    {
                        distribuitorToUpdate.DistributedHammocks.Add(new DistributedHammock
                        {
                            DistribuitorID = distribuitorToUpdate.ID,
                            HammockID = hammock.ID
                        });
                    }
                }
                else
                {
                    if (publishedBooks.Contains(hammock.ID))
                    {
                        DistributedHammock bookToRemove = distribuitorToUpdate.DistributedHammocks.FirstOrDefault(i
                       => i.HammockID == hammock.ID);
                        _context.Remove(bookToRemove);
                    }
                }
            }
        }

        // GET: Distribuitors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribuitor = await _context.Distribuitors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distribuitor == null)
            {
                return NotFound();
            }

            return View(distribuitor);
        }

        // POST: Distribuitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distribuitor = await _context.Distribuitors.FindAsync(id);
            _context.Distribuitors.Remove(distribuitor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistribuitorExists(int id)
        {
            return _context.Distribuitors.Any(e => e.ID == id);
        }
    }
}
