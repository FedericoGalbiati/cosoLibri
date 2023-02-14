using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cosoLibri.Data;
using cosoLibri.Models;

namespace cosoLibri.Controllers
{
    public class LibroController : Controller
    {
        private readonly LibriDbContext _context;

        public LibroController(LibriDbContext context)
        {
            _context = context;
        }

        // GET: Libro
        public async Task<IActionResult> Index()
        {
            var libriDbContext = _context.Libros.Include(l => l.FkEditoreNavigation);
            return View(await libriDbContext.ToListAsync());
        }

        // GET: Libro/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.FkEditoreNavigation)
                .FirstOrDefaultAsync(m => m.Codice == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libro/Create
        public IActionResult Create()
        {
            ViewData["FkEditore"] = new SelectList(_context.Editores, "CodiceEditore", "CodiceEditore");
            return View();
        }

        // POST: Libro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codice,Isbn,Titolo,AnnoPub,Lingua,FkEditore")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkEditore"] = new SelectList(_context.Editores, "CodiceEditore", "CodiceEditore", libro.FkEditore);
            return View(libro);
        }

        // GET: Libro/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["FkEditore"] = new SelectList(_context.Editores, "CodiceEditore", "CodiceEditore", libro.FkEditore);
            return View(libro);
        }

        // POST: Libro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Codice,Isbn,Titolo,AnnoPub,Lingua,FkEditore")] Libro libro)
        {
            if (id != libro.Codice)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Codice))
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
            ViewData["FkEditore"] = new SelectList(_context.Editores, "CodiceEditore", "CodiceEditore", libro.FkEditore);
            return View(libro);
        }

        // GET: Libro/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.FkEditoreNavigation)
                .FirstOrDefaultAsync(m => m.Codice == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'LibriDbContext.Libros'  is null.");
            }
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(uint id)
        {
          return (_context.Libros?.Any(e => e.Codice == id)).GetValueOrDefault();
        }
    }
}
