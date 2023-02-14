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
    public class EditoreController : Controller
    {
        private readonly LibriDbContext _context;

        public EditoreController(LibriDbContext context)
        {
            _context = context;
        }

        // GET: Editore
        public async Task<IActionResult> Index()
        {
              return _context.Editores != null ? 
                          View(await _context.Editores.ToListAsync()) :
                          Problem("Entity set 'LibriDbContext.Editores'  is null.");
        }

        // GET: Editore/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Editores == null)
            {
                return NotFound();
            }

            var editore = await _context.Editores
                .FirstOrDefaultAsync(m => m.CodiceEditore == id);
            if (editore == null)
            {
                return NotFound();
            }

            return View(editore);
        }

        // GET: Editore/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodiceEditore,Nome,Via,Civico,Città,Cap,Provincia,Agente,Telefono,SitoWeb")] Editore editore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editore);
        }

        // GET: Editore/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || _context.Editores == null)
            {
                return NotFound();
            }

            var editore = await _context.Editores.FindAsync(id);
            if (editore == null)
            {
                return NotFound();
            }
            return View(editore);
        }

        // POST: Editore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("CodiceEditore,Nome,Via,Civico,Città,Cap,Provincia,Agente,Telefono,SitoWeb")] Editore editore)
        {
            if (id != editore.CodiceEditore)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditoreExists(editore.CodiceEditore))
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
            return View(editore);
        }

        // GET: Editore/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Editores == null)
            {
                return NotFound();
            }

            var editore = await _context.Editores
                .FirstOrDefaultAsync(m => m.CodiceEditore == id);
            if (editore == null)
            {
                return NotFound();
            }

            return View(editore);
        }

        // POST: Editore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Editores == null)
            {
                return Problem("Entity set 'LibriDbContext.Editores'  is null.");
            }
            var editore = await _context.Editores.FindAsync(id);
            if (editore != null)
            {
                _context.Editores.Remove(editore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditoreExists(uint id)
        {
          return (_context.Editores?.Any(e => e.CodiceEditore == id)).GetValueOrDefault();
        }
    }
}
