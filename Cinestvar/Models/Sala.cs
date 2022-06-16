using System.ComponentModel.DataAnnotations;

namespace Cinestvar.Models
{
    public class Sala
    {
        [Key]
        public int IdSale { get; set; }
        public string NazivSale { get; set; }
        public int BrojRedova { get; set; }
        public int BrojKolona { get; set; }

        public Sala() { }
    }
}
