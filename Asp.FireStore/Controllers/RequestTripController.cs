using Asp.FireStore.Models;
using Asp.FireStore.Models.ViewModel;
using Asp.FireStore.Repository.Implementation;
using Asp.FireStore.Repository.IRepository;
using Firebase.Auth;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Grpc.Core.Metadata;

namespace Asp.FireStore.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class RequestTripController : Controller
    {
        private readonly IRequestTripRepository requestTripRepository;
        private readonly IUsersRepository usersRepository;

        public RequestTripController(IRequestTripRepository requestTripRepository, IUsersRepository usersRepository)
        {
            this.requestTripRepository = requestTripRepository;
            this.usersRepository = usersRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await requestTripRepository.GetAllAsync("TaxiDriverRequests");
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Print()
        {
            var res = await requestTripRepository.GetAllAsync("TaxiDriverRequests");
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var res = await requestTripRepository.GetById("TaxiDriverRequests", id);
            return View(res);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(RequestTrip trip, string id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var res = await usersRepository.GetById("Users", id);

            // تحويل البيانات المدخلة إلى نوع GeoPoint
            string[] currentLocation = Request.Form["currentLocation"].ToString().Split(',');
            trip.CurrentLocation = new GeoPoint(Math.Round(double.Parse(currentLocation[0]), 5),Math.Round(double.Parse(currentLocation[1]), 5));

            string[] targetLocation = Request.Form["targetLocation"].ToString().Split(',');
            trip.TargetLocation = new GeoPoint(Math.Round(double.Parse(targetLocation[0]),5), Math.Round(double.Parse(targetLocation[1]), 5));

            trip.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            trip.Stander.CreatedBy = AdminId;


            trip.UserId = id;
            trip.Status = 0;
            trip.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            trip.Stander.IsDelete = false;
            trip.Stander.IsActive = true;
            await requestTripRepository.AddAsync("TaxiDriverRequests", trip);
            return RedirectToAction("Profile", $"{res.UserType}", new { id = trip.UserId });

        }

        [HttpGet]
        public async Task<IActionResult> Accept(string id,string driverid)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res = await requestTripRepository.GetById("TaxiDriverRequests", id);
            res.DriverId = driverid;
            res.Status = 1;
            res.Stander.ModifiedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            await requestTripRepository.Update("TaxiDriverRequests", id, res);
            return RedirectToAction("Profile", "TaxiDriver", new { id = driverid });

        }


        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            var data = await requestTripRepository.GetById("TaxiDriverRequests", id);
            data.Stander.IsActive = true;
            await requestTripRepository.Update("TaxiDriverRequests", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DisActive(string id)
        {
            var data = await requestTripRepository.GetById("TaxiDriverRequests", id);
            data.Stander.IsActive = false;
            await requestTripRepository.Update("TaxiDriverRequests", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            var data = await requestTripRepository.GetById("TaxiDriverRequests", id);
            data.Stander.IsDelete = true;
            data.Stander.DeletedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.DeletedBy = AdminId;

            await requestTripRepository.Update("TaxiDriverRequests", id, data);
            return RedirectToAction("Index");
        }

    }
}
