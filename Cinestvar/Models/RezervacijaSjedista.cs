using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinestvar.Models
{
    public class RezervacijaSjedista
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Rezervacija")]
        public int IdRezervacije { get; set; }

        public string OznakaSjedista { get; set; }

        public RezervacijaSjedista() { }
    }
}
