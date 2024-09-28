using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Asp.FireStore.Models
{
    [FirestoreData]
    public class Vehicle
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        [Display(Name = "Driver")]
        public string? DriverId { get; set; }
        [FirestoreProperty]
        [Display(Name = "Car Type")]
        public string CarType { get; set; }
        [FirestoreProperty]
        [Display(Name = "Car Model")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "يجب أن يحتوي موديل السيارة على اربعة ارقام.")]
        public string CarModel { get; set; }
        [FirestoreProperty]
        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }
        [FirestoreProperty]
        [Display(Name = "City Key Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{1,2}$", ErrorMessage = "يجب أن يحتوي مفتاح المدينة على الاقل رقم واحد وبالكثير رقمين فقط.")]
        public string CityKey { get; set; }
        [FirestoreProperty]
        [Display(Name = "License Plate Number")]
        public string LicensePlateNumber { get; set; }
        [FirestoreProperty]
        [Display(Name = "Vehicle License Card Image")]
        [DataType(DataType.Upload)]
        public string VehicleLicenseCardImage { get; set; }
        [FirestoreProperty]
        public Stander? Stander { get; set; }
    }
}
