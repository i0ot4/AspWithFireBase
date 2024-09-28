using Google.Cloud.Firestore;

namespace Asp.FireStore.Models.ViewModel
{
    [FirestoreData]
    public class ReportVM
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string UId { get; set; }

        [FirestoreProperty]
        public Reports? Reports { get; set; }
    }
}
