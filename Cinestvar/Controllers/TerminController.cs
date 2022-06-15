using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinestvar.Data;
using Cinestvar.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinestvar.Controllers
{
    [Authorize(Roles = "Radnik")]
    public class TerminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TerminController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        // GET: Termin
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Termin.Include(t => t.Film).Include(t => t.Sala);
            return View(await applicationDbContext.OrderBy(t => t.IdFilma).OrderBy(t=> t.PocetakTermina).
                ToListAsync());
        }

        
        // GET: Termin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termin
                .Include(t => t.Film)
                .Include(t => t.Sala)
                .FirstOrDefaultAsync(m => m.IdTermina == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        
        // GET: Termin/Create
        public IActionResult Create()
        {
            ViewData["IdFilma"] = new SelectList(_context.Film, "IdFilma", "IdFilma");
            ViewData["IdSale"] = new SelectList(_context.Sala, "IdSale", "IdSale");
            return View();
        }

        // POST: Termin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTermina,PocetakTermina,KrajTermina,IdFilma,IdSale")] Termin termin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(termin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFilma"] = new SelectList(_context.Film, "IdFilma", "IdFilma", termin.IdFilma);
            ViewData["IdSale"] = new SelectList(_context.Sala, "IdSale", "IdSale", termin.IdSale);
            return View(termin);
        }


        
        // GET: Termin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termin.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }
            ViewData["IdFilma"] = new SelectList(_context.Film, "IdFilma", "IdFilma", termin.IdFilma);
            ViewData["IdSale"] = new SelectList(_context.Sala, "IdSale", "IdSale", termin.IdSale);
            return View(termin);
        }

        // POST: Termin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTermina,PocetakTermina,KrajTermina,IdFilma,IdSale")] Termin termin)
        {
            if (id != termin.IdTermina)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminExists(termin.IdTermina))
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
            ViewData["IdFilma"] = new SelectList(_context.Film, "IdFilma", "IdFilma", termin.IdFilma);
            ViewData["IdSale"] = new SelectList(_context.Sala, "IdSale", "IdSale", termin.IdSale);
            return View(termin);
        }


        // GET: Termin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termin
                .Include(t => t.Film)
                .Include(t => t.Sala)
                .FirstOrDefaultAsync(m => m.IdTermina == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

       
        // POST: Termin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termin = await _context.Termin.FindAsync(id);
            _context.Termin.Remove(termin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerminExists(int id)
        {
            return _context.Termin.Any(e => e.IdTermina == id);
        }
    }
}
