using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinestvar.Models
{
    public class Termin
    {
        [Key]
        public int IdTermina { get; set; }

        [Display(Name ="Pocetak termina")]
        public DateTime PocetakTermina { get; set; }

        [Display(Name = "Kraj termina")]
        public DateTime KrajTermina { get; set; }

        [ForeignKey("Film")]
        public int IdFilma { get; set; }

        [ForeignKey("Sala")]
        public int IdSale { get; set; }

        public Film Film { get; set; }
        public Sala Sala { get; set; }

        public Termin() { }

    }
}
