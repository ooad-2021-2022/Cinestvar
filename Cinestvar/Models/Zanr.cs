using System.ComponentModel.DataAnnotations;

namespace Cinestvar.Models
{
    public enum Zanr
    {
        [Display(Name = "Akcija")]
        Akcija,
        [Display(Name = "Animirani")]
        Animirani,
        [Display(Name = "Avantura")]
        Avantura,
        [Display(Name = "Drama")]
        Drama,
        [Display(Name = "Fantastika")]
        Fantastika,
        [Display(Name = "Horor")]
        Horor,
        [Display(Name = "Komedija")]
        Komedija,
        [Display(Name = "Porodični")]
        Porodicni,
        [Display(Name = "Romantika")]
        Romantika,
        [Display(Name = "Triler")]
        Triler
    }
}
