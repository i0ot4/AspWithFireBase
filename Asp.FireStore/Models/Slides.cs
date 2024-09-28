using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class Slides
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        [Display(Name = "Image For App Slides")]
        [DataType(DataType.Upload)]
        public string? ImageUrl { get; set; }
        [FirestoreProperty]
        public Stander? Stander { get; set; }
    }
}
