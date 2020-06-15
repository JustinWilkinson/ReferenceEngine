using Bibtex.Abstractions;
using LatexReferences.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatexReferences.Controllers
{
    public class BibliographyStylesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BibliographyStylesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Styles
        public async Task<IActionResult> Index() => View(await _context.BibliographyStyles.ToListAsync());

        // GET: Styles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.BibliographyStyles.Include(x => x.EntryStyles).SingleOrDefaultAsync(m => m.Id == id);
            return style != null ? View(style) as IActionResult : NotFound();
        }

        // GET: Styles/Create
        public IActionResult Create() => View();

        // POST: Styles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] BibliographyStyle style)
        {
            if (ModelState.IsValid)
            {
                _context.Add(style);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = style.Id });
            }
            return View(style);
        }

        // GET: Styles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.BibliographyStyles.Include(x => x.EntryStyles).SingleOrDefaultAsync(x => x.Id == id);
            return style != null ? View(style) as IActionResult : NotFound();
        }

        // POST: Styles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, string entryStyleIds)
        {
            var style = await _context.BibliographyStyles.Include(x => x.EntryStyles).SingleOrDefaultAsync(x => x.Id == id);

            if (style == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "Name cannot be blank!");
            }

            var ids = new List<int>();
            try
            {
                if (entryStyleIds != null)
                {
                    ids.AddRange(entryStyleIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    style.Name = name;
                    style.EntryStyles = await _context.EntryStyles.Where(x => ids.Contains(x.Id)).ToListAsync();
                    _context.Update(style);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StyleExists(style.Id))
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
            return View(style);
        }

        // GET: Styles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.BibliographyStyles.SingleOrDefaultAsync(m => m.Id == id);
            return style != null ? View(style) as IActionResult : NotFound();
        }

        // POST: Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var style = await _context.BibliographyStyles.FindAsync(id);
            _context.BibliographyStyles.Remove(style);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Styles/Export/5
        public async Task<IActionResult> Export(int id)
        {
            var style = await _context.BibliographyStyles.Include(x => x.EntryStyles).SingleOrDefaultAsync(x => x.Id == id);
            var json = JsonConvert.SerializeObject(style, Formatting.Indented);
            return File(Encoding.UTF8.GetBytes(json), "application/json", $"{style.Name}.style.json");
        }

        private bool StyleExists(int id) => _context.BibliographyStyles.Any(e => e.Id == id);
    }
}