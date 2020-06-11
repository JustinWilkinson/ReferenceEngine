using Bibtex.Abstractions;
using LatexReferences.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatexReferences.Controllers
{
    public class EntryStylesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntryStylesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EntryStyles
        public async Task<IActionResult> Index() => View(await _context.EntryStyles.ToListAsync());

        // GET: EntryStyles/List
        public async Task<IEnumerable<EntryStyle>> List() => await _context.EntryStyles.ToListAsync();

        // GET: EntryStyles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryStyle = await _context.EntryStyles.FirstOrDefaultAsync(m => m.Id == id);
            if (entryStyle == null)
            {
                return NotFound();
            }

            return View(entryStyle);
        }

        // GET: EntryStyles/Create
        public IActionResult Create() => View();

        // POST: EntryStyles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] EntryStyle entryStyle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entryStyle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = entryStyle.Id });
            }
            return View(entryStyle);
        }

        // GET: EntryStyles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var (found, value) = await _context.EntryStyles.TryFindAsync(id);
            return found ? NotFound() : (IActionResult)View(value);
        }

        // POST: EntryStyles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string entryStyleString)
        {
            EntryStyle entryStyle = null;
            try
            {
                entryStyle = JsonConvert.DeserializeObject<EntryStyle>(entryStyleString);
                entryStyle.Id = id;
            }
            catch (Exception ex)
            {
                entryStyle = await _context.EntryStyles.FindAsync(id);
                ModelState.AddModelError("", ex.Message);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entryStyle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryStyleExists(entryStyle.Id))
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
            return View(entryStyle);
        }

        // GET: EntryStyles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryStyle = await _context.EntryStyles.FirstOrDefaultAsync(m => m.Id == id);
            if (entryStyle == null)
            {
                return NotFound();
            }

            return View(entryStyle);
        }

        // POST: EntryStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entryStyle = await _context.EntryStyles.FindAsync(id);
            _context.EntryStyles.Remove(entryStyle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntryStyleExists(int id) => _context.EntryStyles.Any(e => e.Id == id);
    }
}