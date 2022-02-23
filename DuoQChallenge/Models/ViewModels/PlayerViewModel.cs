using System.ComponentModel.DataAnnotations;

namespace DuoQChallenge.Models.ViewModels
{
    public class PlayerViewModel
    {
        [Required]
        [Display(Name ="Player Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name= "Player Role")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        public string InGameName { get; set; }
    }
}
