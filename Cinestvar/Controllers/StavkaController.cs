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
    public class StavkaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StavkaController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET za specijalne ponude
        public async Task<IActionResult> SpecijalnePonude()
        {
            return View(await _context.Stavka.Where(stavka => stavka.TipStavke == TipStavke.SpecijalnaPonuda).ToListAsync());

        }

        // GET: Stavka
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stavka.Where(stavka => stavka.TipStavke==TipStavke.Hrana).ToListAsync());
        }


        // GET: Stavka/Details/5
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stavka = await _context.Stavka
                .FirstOrDefaultAsync(m => m.IdStavke == id);
            if (stavka == null)
            {
                return NotFound();
            }

            return View(stavka);
        }

        // GET: Stavka/Create
        [Authorize(Roles = "Radnik")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stavka/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Create([Bind("IdStavke,NazivStavke,OpisStavke,TipStavke")] Stavka stavka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stavka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stavka);
        }

        // GET: Stavka/Edit/5
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stavka = await _context.Stavka.FindAsync(id);
            if (stavka == null)
            {
                return NotFound();
            }
            return View(stavka);
        }

        // POST: Stavka/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Edit(int id, [Bind("IdStavke,NazivStavke,OpisStavke,TipStavke")] Stavka stavka)
        {
            if (id != stavka.IdStavke)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stavka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StavkaExists(stavka.IdStavke))
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
            return View(stavka);
        }

        // GET: Stavka/Delete/5
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stavka = await _context.Stavka
                .FirstOrDefaultAsync(m => m.IdStavke == id);
            if (stavka == null)
            {
                return NotFound();
            }

            return View(stavka);
        }

        // POST: Stavka/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stavka = await _context.Stavka.FindAsync(id);
            _context.Stavka.Remove(stavka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StavkaExists(int id)
        {
            return _context.Stavka.Any(e => e.IdStavke == id);
        }
    }
}
