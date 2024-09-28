using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class RequestTrip
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }
        [FirestoreProperty]
        public string? UserId { get; set; }
        [FirestoreProperty]
        public string? DriverId { get; set; }
        [Display(Name = "Vehicle used")]
        [FirestoreProperty]
        public string? VehicleId { get; set; }
        [Display(Name = "Current Location Position")]
        [FirestoreProperty]
        public GeoPoint CurrentLocation { get; set; }
        [Display(Name = "Current Location Name")]
        [FirestoreProperty]
        public string? CurrentLocationName { get; set; }
        [Display(Name = "Target Location Position")]
        [FirestoreProperty]
        public GeoPoint TargetLocation { get; set; }
        [Display(Name = "Target Location Name")]
        [FirestoreProperty]
        public string? TargetLocationName { get; set; }
        [Display(Name = "Details")]
        [FirestoreProperty]
        public string? Details { get; set; }
        [Display(Name = "Status")]
        [FirestoreProperty]
        public int? Status { get; set; }
        [Display(Name = "AccpetAt")]
        [FirestoreProperty]
        public Timestamp? AcceptAt { get; set; }
        [FirestoreProperty]
        public Stander? Stander { get; set; }
        [FirestoreProperty]
        public bool? Reported { get; set; }
    }
}
