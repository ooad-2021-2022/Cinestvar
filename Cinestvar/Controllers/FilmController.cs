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
    public class FilmController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Film
        public async Task<IActionResult> Index()
        {
            return View(await _context.Film.ToListAsync());
        }

        // GET: Film/Details/5
        public async Task<IActionResult> Details(int? id, DateTime? datum)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.IdFilma == id);
            if (film == null)
            {
                return NotFound();
            }

            if (datum == null)
                datum = DateTime.Now;

            List<Termin> lista = await _context.Termin.Where(
                t => t.IdFilma == id &&
                t.PocetakTermina > ((DateTime)datum).AddMinutes(59)
                && t.PocetakTermina<((DateTime)datum).AddDays(1)).ToListAsync();
            List<Termin> lista2 = await _context.Termin.Where(
                t => t.IdFilma == id &&
                t.PocetakTermina > ((DateTime)datum).AddMinutes(59)
                ).ToListAsync();

            if (lista.Count == 0)
                return View("NemaTermina", film);

            foreach (var termin in lista2)
            {
                if (Popunjen(termin).Result)
                    lista.Remove(termin);
            }

            
            //ovdje ide id sale za pravna lica
            var idSalePravno = 1234;
            if (User.IsInRole("Pravno lice"))
                lista = lista.Where(t => t.IdSale == idSalePravno).ToList();
            else
                lista = lista.Where(t => t.IdSale != idSalePravno).ToList();
           
            if (lista.Count == 0)
                return View("NemaTermina", film);
            return View(lista);
        }

        private async Task<bool> Popunjen(Termin termin)
        {
            List<Rezervacija> rezervacije = await _context.Rezervacija.Where(
                rez => rez.IdTermina==termin.IdTermina
                ).ToListAsync();
            Sala sala = await _context.Sala.FindAsync(termin.IdSale);
            //umjesto hala sala ide naziv sale za pravna lica
            if (rezervacije.Count()!=0 && sala.NazivSale == "hala sala")
                return true;
            var kapacitet = sala.BrojKolona * sala.BrojRedova;
            return rezervacije.Count() == kapacitet;
        }

        // GET: Film/Create
        [Authorize(Roles = "Radnik")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Film/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Create([Bind("IdFilma,NazivFilma,PosterLink,Trajanje,Zanr,Opis,RecenzijaLink,CijenaKarte")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Film/Edit/5
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Film/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Edit(int id, [Bind("IdFilma,NazivFilma,PosterLink,Trajanje,Zanr,Opis,RecenzijaLink,CijenaKarte")] Film film)
        {
            if (id != film.IdFilma)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.IdFilma))
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
            return View(film);
        }

        // GET: Film/Delete/5
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.IdFilma == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Film/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Radnik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Film.FindAsync(id);
            _context.Film.Remove(film);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Film.Any(e => e.IdFilma == id);
        }



        public async Task<IActionResult> Filter()
        {

            return View(await _context.Stavka.Where(stavka => stavka.TipStavke == TipStavke.SpecijalnaPonuda).ToListAsync());

        }
    }

}
