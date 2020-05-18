using LatexReferences.Models;
using LatexReferences.Models.Format;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LatexReferences.Controllers
{
    public class FormatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Formats
        public async Task<IActionResult> Index() => View(await _context.BibEntryFormats.ToListAsync());

        // GET: Formats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bibEntryFormat = await _context.BibEntryFormats.FirstOrDefaultAsync(m => m.Id == id);
            if (bibEntryFormat == null)
            {
                return NotFound();
            }

            return View(bibEntryFormat);
        }

        // GET: Formats/Create
        public IActionResult Create() => View();

        // POST: Formats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IncludeAddress,IncludeAnnote,IncludeAuthor,IncludeBooktitle,IncludeChapter,IncludeCrossReference,IncludeDOI,IncludeEdition,IncludeEditor,IncludeEmail,IncludeHowpublished,IncludeInstitution,IncludeJournal,IncludeKey,IncludeMonth,IncludeNumber,IncludeOrganization,IncludePages,IncludePublisher,IncludeSchool,IncludeSeries,IncludeTitle,IncludeType,IncludeVolume,IncludeYear")] BibEntryFormat bibEntryFormat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bibEntryFormat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bibEntryFormat);
        }

        // GET: Formats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bibEntryFormat = await _context.BibEntryFormats.FindAsync(id);
            if (bibEntryFormat == null)
            {
                return NotFound();
            }
            return View(bibEntryFormat);
        }

        // POST: Formats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IncludeAddress,IncludeAnnote,IncludeAuthor,IncludeBooktitle,IncludeChapter,IncludeCrossReference,IncludeDOI,IncludeEdition,IncludeEditor,IncludeEmail,IncludeHowpublished,IncludeInstitution,IncludeJournal,IncludeKey,IncludeMonth,IncludeNumber,IncludeOrganization,IncludePages,IncludePublisher,IncludeSchool,IncludeSeries,IncludeTitle,IncludeType,IncludeVolume,IncludeYear")] BibEntryFormat bibEntryFormat)
        {
            if (id != bibEntryFormat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bibEntryFormat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BibEntryFormatExists(bibEntryFormat.Id))
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
            return View(bibEntryFormat);
        }

        // GET: Formats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bibEntryFormat = await _context.BibEntryFormats.FirstOrDefaultAsync(m => m.Id == id);
            if (bibEntryFormat == null)
            {
                return NotFound();
            }

            return View(bibEntryFormat);
        }

        // POST: Formats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bibEntryFormat = await _context.BibEntryFormats.FindAsync(id);
            _context.BibEntryFormats.Remove(bibEntryFormat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BibEntryFormatExists(int id) => _context.BibEntryFormats.Any(e => e.Id == id);
    }
}