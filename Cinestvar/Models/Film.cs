using System.ComponentModel.DataAnnotations;

namespace Cinestvar.Models
{
    public class Film
    {
        [Key]
        public int IdFilma { get; set; }
        [Display(Name ="")]
        public string NazivFilma { get; set; }
        [Display (Name ="")]
        public string PosterLink { get; set; }
        public int Trajanje { get; set; }

        [Display (Name ="Žanr")]
        [EnumDataType(typeof(Zanr))]
        public Zanr Zanr { get; set; }
        public string Opis { get; set; }
        public string RecenzijaLink { get; set; }

        [Display (Name = "Cijena karte")]
        public double CijenaKarte { get; set; }

        public Film() { }

    }
}
