using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinestvar.Models
{
    public class Rezervacija
    {
        [Key]
        public int IdRezervacije { get; set; }

        [ForeignKey("Termin")]
        public int IdTermina { get; set; }

        [ForeignKey("AspNetUsers")]
        public int IdKorisnika { get; set; }

        public Termin Termin { get; set; }
        public IdentityUser IdentityUser { get; set; }

        public Rezervacija() { }

    }
}
