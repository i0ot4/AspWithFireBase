using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models.FirebaseModel
{
    public class ChangePasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_])[A-Za-z\d@$!%*?&_]{8,}$",
         ErrorMessage = "كلمة المرور يجب أن تحتوي على الأقل على حرف صغير واحد، حرف كبير واحد، رقم واحد، ورمز خاص واحد، وأن يكون طولها 8 أحرف على الأقل.")]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
