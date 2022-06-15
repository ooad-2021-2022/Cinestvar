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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Cinestvar.Controllers
{
    [Authorize]
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public int brojkarata = -1;
        public Termin termin=null;
        public Rezervacija rez = new Rezervacija();

        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rezervacija
        

        public async Task<IActionResult> BrojKarata(int id)
        {
            termin = await _context.Termin.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }
            termin.IdTermina = id;
            var film = await _context.Film.FindAsync(termin.IdFilma);
            termin.Film = film;
           
            return View(termin);
        }
        //[HttpPost]
        /*public async Task<IActionResult> Test()
        {
            brojkarata = Int32.Parse(Request.Form["test"].First()); //sa test text boxa u BrojKarata view
            if (brojkarata != 6) return NotFound();
            if (!SlobodanTermin(brojkarata)) return View("AlternativniTermin", rez); //da li ovaj view ide u termine??
            return View("Create", rez); //vrati me gdje sam bio 
        }*/

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

       
        // GET: Rezervacija/Create
        public async Task<IActionResult> Create(int? id)
        {
            //if (!SlobodanTermin((int)brojkarata)) return View("AlternativniTermin", rez); //da li ovaj view ide u termine?
            termin = await _context.Termin.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }
            termin.IdTermina = (int)id;
            var film = await _context.Film.FindAsync(termin.IdFilma);
            termin.Film = film;
           
            
            ViewData["Termin"] = termin; 
            return View(termin);
        }

        // POST: Rezervacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRezervacije,IdTermina,IdKorisnika")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Termin"] = termin;
            return View(rezervacija);
        }
        */
        // GET: Rezervacija/Edit/5
      

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

        //private Task<IdentityUser> GetCurrentUserAsync() => UserManager.GetUserAsync(HttpContext.User);
       

        public async Task<IActionResult> PotvrdiFizicko(int id)
        {
            Rezervacija rezervacija = new Rezervacija();
            rezervacija.Termin= termin;
            rezervacija.IdTermina = id;
            
            //String korisnik = Environment.UserName;
            //rezervacija.IdentityUser.Id = korisnik;

            //var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            rezervacija.IdKorisnika = 1;
            _context.Add(rezervacija);
            await _context.SaveChangesAsync();

            RezervacijaSjedista rezsjedista = new RezervacijaSjedista();
            rezsjedista.IdRezervacije = rezervacija.IdRezervacije;
            rezsjedista.OznakaSjedista = "ozn";
            _context.Add(rezsjedista);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Film");
        }

        public async Task<IActionResult> PotvrdiPravno(int id)
        {
            Rezervacija rezervacija = new Rezervacija();
            rezervacija.Termin = termin;
            rezervacija.IdTermina = id;

            //String korisnik = Environment.UserName;
            //rezervacija.IdentityUser.Id = korisnik;

            //var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            rezervacija.IdKorisnika = 1;
            _context.Add(rezervacija);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Film");
        }
    }
}
