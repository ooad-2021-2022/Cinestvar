using System.ComponentModel.DataAnnotations;

namespace Cinestvar.Models
{
    public class Stavka
    {
        [Key]
        public int IdStavke { get; set; }
        public string NazivStavke { get; set; }
        public string OpisStavke { get; set; }
        public TipStavke TipStavke { get; set; }

        public Stavka() { }
    }
}
