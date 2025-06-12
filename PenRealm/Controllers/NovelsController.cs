using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PenRealm.DataAccess;
using PenRealm.Models;

namespace PenRealm.Controllers
{
    [Authorize]
    public class NovelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NovelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var novels = await _context.Novels
                .Where(n => n.UserId == user.Id)
                .ToListAsync();

            return View(novels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Novel novel)
        {
            var user = await _userManager.GetUserAsync(User);

            novel.UserId = user.Id;
            novel.CreatedAt = DateTime.UtcNow;

            _context.Novels.Add(novel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var novel = await _context.Novels
                .Include(n => n.Chapters)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (novel == null || novel.UserId != _userManager.GetUserId(User))
                return NotFound();

            return View(novel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var novel = await _context.Novels.FindAsync(id);
            var userId = _userManager.GetUserId(User);
            if (novel == null || novel.UserId != userId)
                return NotFound();

            return View(novel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Novel novel)
        {
            var userId = _userManager.GetUserId(User);
            if (id != novel.Id || novel.UserId != userId)
                return Unauthorized();


            try
            {
                _context.Update(novel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Novels.Any(e => e.Id == novel.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var novel = await _context.Novels
                .Include(n => n.Chapters)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (novel == null || novel.UserId != _userManager.GetUserId(User))
                return NotFound();

            return View(novel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var novel = await _context.Novels
                .Include(n => n.Chapters)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (novel == null || novel.UserId != _userManager.GetUserId(User))
                return NotFound();

            _context.Chapters.RemoveRange(novel.Chapters);
            _context.Novels.Remove(novel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
