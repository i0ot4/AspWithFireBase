using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models.FirebaseModel
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
