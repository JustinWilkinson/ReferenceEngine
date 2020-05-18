using LatexReferences.Models;
using LatexReferences.Models.Format;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LatexReferences.Controllers
{
    public class FullFormatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FullFormatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FullFormats
        public async Task<IActionResult> Index() => View(await _context.FullFormats.ToListAsync());

        // GET: FullFormats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fullFormat = await _context.FullFormats.FirstOrDefaultAsync(m => m.Id == id);
            if (fullFormat == null)
            {
                return NotFound();
            }

            return View(fullFormat);
        }

        // GET: FullFormats/Create
        public IActionResult Create() => View();

        // POST: FullFormats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] FullFormat fullFormat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fullFormat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fullFormat);
        }

        // GET: FullFormats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fullFormat = await _context.FullFormats.FindAsync(id);
            if (fullFormat == null)
            {
                return NotFound();
            }
            return View(fullFormat);
        }

        // POST: FullFormats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FullFormat fullFormat)
        {
            if (id != fullFormat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fullFormat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FullFormatExists(fullFormat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fullFormat);
        }

        // GET: FullFormats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fullFormat = await _context.FullFormats.FirstOrDefaultAsync(m => m.Id == id);
            if (fullFormat == null)
            {
                return NotFound();
            }

            return View(fullFormat);
        }

        // POST: FullFormats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fullFormat = await _context.FullFormats.FindAsync(id);
            _context.FullFormats.Remove(fullFormat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FullFormatExists(int id) => _context.FullFormats.Any(e => e.Id == id);
    }
}