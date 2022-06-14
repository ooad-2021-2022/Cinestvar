

using System.ComponentModel.DataAnnotations;

namespace Cinestvar.Models
{
    public enum TipStavke
    {
        [Display(Name="Hrana")]
        Hrana,
        [Display(Name = "Specijalna ponuda")]
        SpecijalnaPonuda
    }
}
