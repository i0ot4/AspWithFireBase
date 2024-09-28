using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class RequestMechanic
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }
        [FirestoreProperty]
        public string? UserId { get; set; }
        [FirestoreProperty]
        public string? MechanicId { get; set; }
        [FirestoreProperty]
        [Display(Name = "Current Location")]
        public GeoPoint CurrentLocation { get; set; }
        [FirestoreProperty]
        [Display(Name = "Details")]
        public string? Details { get; set; }

        [Display(Name = "AccpetAt")]
        [FirestoreProperty]
        public Timestamp? AcceptAt { get; set; }
        [FirestoreProperty]
        [Display(Name = "Car Type")]
        public string? CarType { get; set; }
        [FirestoreProperty]
        [Display(Name = "Car Model")]
        public string? CarModel { get; set; }
        [FirestoreProperty]
        [Display(Name = "Fuel Type")]
        public string? FuelType { get; set; }   // نوع وقود السياره (بترول - ديزل) فقط
        [FirestoreProperty]
        [Display(Name = "Status")]
        public int? Status { get; set; }
        [FirestoreProperty]
        public Stander? Stander { get; set; }
        [FirestoreProperty]
        public bool? Reported { get; set; }
    }
}
