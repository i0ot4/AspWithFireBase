using Google.Cloud.Firestore;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class Reports
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }
        [FirestoreProperty]
        public string? UserId { get; set; }
        [FirestoreProperty]
        public string? RequestId { get; set; }
        [FirestoreProperty]
        public string RequestType { get; set; }
        [FirestoreProperty]
        public string Details { get; set; }
        [FirestoreProperty]
        public int? Status { get; set; } //0 waiting or 1 read
        [FirestoreProperty]
        public Stander? Stander { get; set; }

    }
}
