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
    public class RequestPoliceController : Controller
    {
        private readonly IRequestPoliceRepository _requestPoliceRepository;
        private readonly IUsersRepository _usersRepository;

        public RequestPoliceController(IRequestPoliceRepository requestPoliceRepository, IUsersRepository usersRepository)
        {
            _requestPoliceRepository = requestPoliceRepository;
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _requestPoliceRepository.GetAllAsync("TrafficPoliceRequests");
            return View(res);
        }
        
        [HttpGet]
        public async Task<IActionResult> Print()
        {
            var res = await _requestPoliceRepository.GetAllAsync("TrafficPoliceRequests");
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var res = await _requestPoliceRepository.GetById("TrafficPoliceRequests", id);
            return View(res);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RequestPolice police, string id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string[] targetLocation = Request.Form["currentLocation"].ToString().Split(',');
            police.CurrentLocation = new GeoPoint(Math.Round(double.Parse(targetLocation[0]), 5), Math.Round(double.Parse(targetLocation[1]), 5));

            var res = await _usersRepository.GetById("Users",id);
            police.UserId = id;
            police.Status = 0;
            police.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            police.Stander.CreatedBy = AdminId;

            police.Stander.IsDelete = false;
            police.Stander.IsActive = true;
            await _requestPoliceRepository.AddAsync("TrafficPoliceRequests",police);
            return RedirectToAction("Profile",$"{res.UserType}",new {id = police.UserId});

        }

        [HttpGet]
        public async Task<IActionResult> Accept(string id, string policeid)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res = await _requestPoliceRepository.GetById("TrafficPoliceRequests", id);
            res.PoliceId = policeid;
            res.Status = 1;
            res.Stander.ModifiedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            await _requestPoliceRepository.Update("TrafficPoliceRequests", id, res);
            return RedirectToAction("Profile", "TrafficPolice", new { id = policeid });

        }


        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            var data = await _requestPoliceRepository.GetById("TrafficPoliceRequests", id);
            data.Stander.IsActive = true;
            await _requestPoliceRepository.Update("TrafficPoliceRequests", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DisActive(string id)
        {
            var data = await _requestPoliceRepository.GetById("TrafficPoliceRequests", id);
            data.Stander.IsActive = false;
            await _requestPoliceRepository.Update("TrafficPoliceRequests", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            var data = await _requestPoliceRepository.GetById("TrafficPoliceRequests", id);
            data.Stander.IsDelete = true;
            data.Stander.DeletedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.DeletedBy = AdminId;

            await _requestPoliceRepository.Update("TrafficPoliceRequests", id, data);
            return RedirectToAction("Index");
        }

    }
}
