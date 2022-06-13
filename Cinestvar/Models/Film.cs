using System.ComponentModel.DataAnnotations;

namespace Cinestvar.Models
{
    public class Film
    {
        [Key]
        public int IdFilma { get; set; }
        public string NazivFilma { get; set; }
        public string PosterLink { get; set; }
        public int Trajanje { get; set; }
        public Zanr Zanr { get; set; }
        public string Opis { get; set; }
        public string RecenzijaLink { get; set; }
        public double CijenaKarte { get; set; }

        public Film() { }

    }
}
