using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class Users
    {

        [FirestoreDocumentId]
        public string? Id { get; set; }
        [FirestoreProperty]
        public string? DeviceToken { get; set; }
        [Required(ErrorMessage = "Please enter the User First Name")]
        [Display(Name = "First Name")]
        [FirestoreProperty]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter the User Last Name")]
        [Display(Name = "Last Name")]
        [FirestoreProperty]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter the User Address")]
        [Display(Name = "Address")]
        [FirestoreProperty]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter the User Email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_])[A-Za-z\d@$!%*?&_]{8,}$",
         ErrorMessage = "كلمة المرور يجب أن تحتوي على الأقل على حرف صغير واحد، حرف كبير واحد، رقم واحد، ورمز خاص واحد، وأن يكون طولها 8 أحرف على الأقل.")]
        public string Password { get; set; }

        [Display(Name = "Profile Image")]
        [FirestoreProperty]
        public string? ProfilePicture { get; set; }
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber, ErrorMessage = ".هذا الحقل يجب ان يحتوي على ارقام فقط")]
        [FirestoreProperty]
        public string PhoneNumber { get; set; }
        [Display(Name = "User Type")]
        [FirestoreProperty]
        public string UserType { get; set; }
        [Display(Name = "Driver License photo")]
        [FirestoreProperty]
        public string? DriverLicense { get; set; }
        [Display(Name = "The Front Of Personal Card photo")]
        [FirestoreProperty]
        public string? PersonalCardFront { get; set; }
        [Display(Name = "The Back Of Personal Card photo")]
        [FirestoreProperty]
        public string? PersonalCardBack { get; set; }
        [Display(Name = "Center Name")]
        [FirestoreProperty]
        public string? CenterName { get; set; }
        [Display(Name = "Center Location")]
        [FirestoreProperty]
        public GeoPoint? CenterLocation { get; set; }
        [Display(Name = "Rental License")]
        [FirestoreProperty]
        public string? RentalLicense { get; set; }
        [Display(Name = "The Policer Card Image")]
        [FirestoreProperty]
        public string? PolicerCardImage { get; set; }
        [FirestoreProperty]
        public Stander? Stander { get; set; }
    }
}
