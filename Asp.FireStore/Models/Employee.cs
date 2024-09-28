using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class Employee
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }
        [FirestoreProperty]
        [Display(Name ="Mechanic")]
        public string? MechanicId { get; set; }
        [FirestoreProperty]
        public string? Name { get; set; }
        [FirestoreProperty]
        public int? Age { get; set; }
        [FirestoreProperty]
        public string? City { get; set; }
        [FirestoreProperty]
        public string? ProfilePicture { get; set; }
        [FirestoreProperty]
        public string? Phone { get; set; }
        [FirestoreProperty]
        public Stander? Stander { get; set; }
    }
}
