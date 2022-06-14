using System.ComponentModel.DataAnnotations;

namespace Cinestvar.Models
{
    public class Stavka
    {
        [Key]
        public int IdStavke { get; set; }

        [Display(Name ="Naziv")]
        public string NazivStavke { get; set; }

        [Display(Name = "Opis")]
        public string OpisStavke { get; set; }

        [EnumDataType(typeof(TipStavke))]
        public TipStavke TipStavke { get; set; }

        public Stavka() { }
    }
}
