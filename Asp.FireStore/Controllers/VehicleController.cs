using Asp.FireStore.Models;
using Asp.FireStore.Repository.Implementation;
using Asp.FireStore.Repository.IRepository;
using Firebase.Auth.Repository;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.FireStore.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUsersRepository _usersRepository;

        public VehicleController(IVehicleRepository vehicleRepository, IUsersRepository usersRepository)
        {
            _vehicleRepository = vehicleRepository;
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _vehicleRepository.GetAllAsync("Vehicle");
            return View(res);
        }
        

        [HttpGet]
        public async Task<IActionResult> Print()
        {
            var res = await _vehicleRepository.GetAllAsync("Vehicle");
            return View(res);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Vehicle vehicle, string id, IFormFile formFile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            vehicle.DriverId = id;
            var uplaod = await _vehicleRepository.UploadImageAsync($"images/vehicles/{id}", formFile);
            vehicle.VehicleLicenseCardImage = uplaod;
            vehicle.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            vehicle.Stander.CreatedBy = AdminId;

            vehicle.Stander.IsActive = true;
            await _vehicleRepository.AddAsync("Vehicle", vehicle);
            return RedirectToAction("Profile", "TaxiDriver", new {id = id});
        }


        [HttpGet]
        public IActionResult AddTo()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTo(Vehicle vehicle, IFormFile formFile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var uplaod = await _vehicleRepository.UploadImageAsync($"images/vehicles/{vehicle.DriverId}", formFile);
            vehicle.VehicleLicenseCardImage = uplaod;
            vehicle.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            vehicle.Stander.CreatedBy = AdminId;

            vehicle.Stander.IsActive = true;
            await _vehicleRepository.AddAsync("Vehicle", vehicle);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var res = await _vehicleRepository.GetById("Vehicle", id);
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string id)
        {
            var data = await _vehicleRepository.GetById("Vehicle", id);
            data.Stander.IsConfirm = true;

            data.Stander.ConfirmAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.ConfirmBy = AdminId;

            await _vehicleRepository.Update("Vehicle", id, data);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            var data = await _vehicleRepository.GetById("Vehicle", id);
            data.Stander.IsActive = true;
            await _vehicleRepository.Update("Vehicle", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DisActive(string id)
        {
            var data = await _vehicleRepository.GetById("Vehicle", id);
            data.Stander.IsActive = false;

            data.Stander.BlockedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.BlockedBy = AdminId;

            await _vehicleRepository.Update("Vehicle", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            var data = await _vehicleRepository.GetById("Vehicle", id);
            data.Stander.IsDelete = true;

            data.Stander.DeletedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.DeletedBy = AdminId;

            await _vehicleRepository.Update("Vehicle", id, data);
            return RedirectToAction("Index");
        }
    }
}
