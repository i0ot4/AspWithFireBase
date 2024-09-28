using Google.Cloud.Firestore;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class Stander
    {
        [FirestoreProperty]
        public string? CreatedBy { get; set; }
        [FirestoreProperty]
        public Timestamp? CreatedAt { get; set; }
        [FirestoreProperty]
        public string? ModifiedBy { get; set; }
        [FirestoreProperty]
        public Timestamp? ModifiedAt { get; set; }
        [FirestoreProperty]
        public string? BlockedBy { get; set; }
        [FirestoreProperty]
        public Timestamp? BlockedAt { get; set; }
        [FirestoreProperty]
        public string? ReadBy { get; set; }
        [FirestoreProperty]
        public Timestamp? ReadAt { get; set; }
        [FirestoreProperty]
        public string? DeletedBy { get; set; }
        [FirestoreProperty]
        public Timestamp? DeletedAt { get; set; }
        [FirestoreProperty]
        public string? ConfirmBy { get; set; }
        [FirestoreProperty]
        public Timestamp? ConfirmAt { get; set; }
        [FirestoreProperty]
        public bool IsConfirm { get; set; }
        [FirestoreProperty]
        public bool IsActive { get; set; }
        [FirestoreProperty]
        public bool IsDelete { get; set; }
    }
}
