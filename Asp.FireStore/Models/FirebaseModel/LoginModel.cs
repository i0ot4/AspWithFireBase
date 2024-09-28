using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models.FirebaseModel
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
