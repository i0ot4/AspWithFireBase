using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class RequestPolice
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }
        [FirestoreProperty]
        public string? UserId { get; set; }
        [FirestoreProperty]
        public string? PoliceId { get; set; }
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
        [Display(Name = "Status")]
        public int? Status { get; set; }
        [FirestoreProperty]
        public Stander? Stander { get; set; }
        [FirestoreProperty]
        public bool? Reported { get; set; }
    }
}
