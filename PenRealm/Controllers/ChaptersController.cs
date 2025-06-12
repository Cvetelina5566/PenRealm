using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PenRealm.DataAccess;
using PenRealm.Models;

namespace PenRealm.Controllers
{
    [Authorize]
    public class ChaptersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChaptersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int novelId)
        {
            var novel = await _context.Novels
                .Include(n => n.Chapters)
                .FirstOrDefaultAsync(n => n.Id == novelId);

            if (novel == null)
                return NotFound();

            ViewBag.NovelId = novel.Id;
            ViewBag.NovelTitle = novel.Title;

            return View(novel.Chapters.OrderBy(c => c.CreatedAt).ToList());
        }

        public IActionResult Create(int novelId)
        {
            ViewBag.NovelId = novelId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int novelId, Chapter chapter)
        {
            

            chapter.NovelId = novelId;
            chapter.CreatedAt = DateTime.UtcNow;

            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Novels", new { id = novelId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
                return NotFound();

            return View(chapter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Chapter chapter)
        {
            if (id != chapter.Id)
                return NotFound();

            try
            {
                chapter.CreatedAt = DateTime.UtcNow;
                _context.Update(chapter);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Chapters.Any(e => e.Id == chapter.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction("Details", "Novels", new { id = chapter.NovelId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Novel)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chapter == null)
                return NotFound();

            return View(chapter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
                return NotFound();

            var novelId = chapter.NovelId;

            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Novels", new { id = novelId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var chapter = await _context.Chapters
                .Include(c => c.Novel)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chapter == null)
                return NotFound();

            return View(chapter);
        }


    }
}
