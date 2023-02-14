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
    public class AutoreController : Controller
    {
        private readonly LibriDbContext _context;

        public AutoreController(LibriDbContext context)
        {
            _context = context;
        }

        // GET: Autore
        public async Task<IActionResult> Index()
        {
              return _context.Autores != null ? 
                          View(await _context.Autores.ToListAsync()) :
                          Problem("Entity set 'LibriDbContext.Autores'  is null.");
        }

        // GET: Autore/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autore = await _context.Autores
                .FirstOrDefaultAsync(m => m.CodiceAutore == id);
            if (autore == null)
            {
                return NotFound();
            }

            return View(autore);
        }

        // GET: Autore/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodiceAutore,Nome,Cognome,Nazione")] Autore autore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autore);
        }

        // GET: Autore/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autore = await _context.Autores.FindAsync(id);
            if (autore == null)
            {
                return NotFound();
            }
            return View(autore);
        }

        // POST: Autore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("CodiceAutore,Nome,Cognome,Nazione")] Autore autore)
        {
            if (id != autore.CodiceAutore)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutoreExists(autore.CodiceAutore))
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
            return View(autore);
        }

        // GET: Autore/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autore = await _context.Autores
                .FirstOrDefaultAsync(m => m.CodiceAutore == id);
            if (autore == null)
            {
                return NotFound();
            }

            return View(autore);
        }

        // POST: Autore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Autores == null)
            {
                return Problem("Entity set 'LibriDbContext.Autores'  is null.");
            }
            var autore = await _context.Autores.FindAsync(id);
            if (autore != null)
            {
                _context.Autores.Remove(autore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutoreExists(uint id)
        {
          return (_context.Autores?.Any(e => e.CodiceAutore == id)).GetValueOrDefault();
        }
    }
}
