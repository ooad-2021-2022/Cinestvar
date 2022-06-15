using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinestvar.Data;
using Cinestvar.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Cinestvar.Controllers
{
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public int brojkarata = -1;
        public Termin termin=null;
        
        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Landing(int id)
        {
         
            termin = await _context.Termin.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }
            Rezervacija rez = new Rezervacija();
            rez.IdTermina = id;
            //var popis = await _context.Termin.ToListAsync();
            //if(popis.Count()==2) return NotFound();
            //termin = await _context.Termin.FirstAsync(ter => ter.IdTermina.ToString() == idterm.ToString());
            //if (termin != null) return NotFound();//prekopiraj termin u lokalnu varijablu (je li ovo plitka kopija?)
            if (KorisnikFizicko() && brojkarata==-1) //ima u funkciji nize redirect, zato provjerava i broj karata
            {
                return View("BrojKarata", rez); //daj prozor za unos broja
            }
            return View("Create", rez); //daj prozor za potvrdu rezervacije
        }

        public async Task<IActionResult> BrojKarata()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Test()
        {
            brojkarata = Int32.Parse(Request.Form["test"].First()); //sa test text boxa u BrojKarata view

            if (!SlobodanTermin(brojkarata)) return RedirectToAction("AlternativniTermin"); //da li ovaj view ide u termine??
            return RedirectToAction("Landing"); //vrati me gdje sam bio 
        }

        private bool SlobodanTermin(int brojkarata) //provjerava ima li dovoljno slobodnih sjedista 
        {
            var sala = _context.Sala.Where(sala => sala.IdSale == termin.IdSale).ToList().ElementAt(0);
            var zauzetitermin = _context.Rezervacija.Where(rez => rez.IdTermina == termin.IdTermina).ToList();
            int zauzeto = 0;
            foreach (var t in zauzetitermin)
            {
                var zauzetasjedista = _context.RezervacijaSjedista.Where(broj => broj.IdRezervacije == t.IdRezervacije).ToList();
                zauzeto = zauzeto + zauzetasjedista.Count();
            }            
            if(zauzeto + brojkarata > (sala.BrojRedova * sala.BrojKolona))
            {
                return false;
            }
            return true;
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Termin)
                .FirstOrDefaultAsync(m => m.IdRezervacije == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }
       
        // GET: Rezervacija/Create
        public IActionResult Create()
        {
            ViewData["IdTermina"] = new SelectList(_context.Termin, "IdTermina", "IdTermina");
            //ovdje bi se automatski trebale popuniti vrijednosti iz termina termin koji je proslijedjen 
            return View();
        }

        // POST: Rezervacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRezervacije,IdTermina,IdKorisnika")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTermina"] = new SelectList(_context.Termin, "IdTermina", "IdTermina", rezervacija.IdTermina);
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            ViewData["IdTermina"] = new SelectList(_context.Termin, "IdTermina", "IdTermina", rezervacija.IdTermina);
            return View(rezervacija);
        }
       
        // POST: Rezervacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRezervacije,IdTermina,IdKorisnika")] Rezervacija rezervacija)
        {
            if (id != rezervacija.IdRezervacije)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.IdRezervacije))
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
            ViewData["IdTermina"] = new SelectList(_context.Termin, "IdTermina", "IdTermina", rezervacija.IdTermina);
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Termin)
                .FirstOrDefaultAsync(m => m.IdRezervacije == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            _context.Rezervacija.Remove(rezervacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.IdRezervacije == id);
        }

        private bool KorisnikFizicko()
        {
            if (User.IsInRole("Fizicko lice")) return true;
            return false;
        }
    }
}
