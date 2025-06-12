using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PenRealm.DataAccess;
using PenRealm.Models;
using Microsoft.EntityFrameworkCore;

namespace PenRealm.Controllers
{
    [Authorize]
    public class PoemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PoemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Poems
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var poems = await _context.Poems
                .Where(p => p.AuthorId == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(poems);
        }

        // GET: /Poems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Poems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Poem poem)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(); 

            poem.AuthorId = user.Id;

            poem.AuthorId = user.Id;
            poem.CreatedAt = DateTime.UtcNow;

            _context.Poems.Add(poem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Poems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var poem = await _context.Poems.FindAsync(id);
            if (poem == null || poem.AuthorId != _userManager.GetUserId(User))
                return NotFound();

            return View(poem);
        }

        // POST: /Poems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Poem updatedPoem)
        {
            var poem = await _context.Poems.FindAsync(id);
            if (poem == null || poem.AuthorId != _userManager.GetUserId(User))
                return NotFound();

            poem.Title = updatedPoem.Title;
            poem.Content = updatedPoem.Content;
            poem.IsPublic = updatedPoem.IsPublic;
            poem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: /Poems/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var poem = await _context.Poems.FindAsync(id);
            if (poem == null || poem.AuthorId != _userManager.GetUserId(User))
                return NotFound();

            _context.Poems.Remove(poem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
