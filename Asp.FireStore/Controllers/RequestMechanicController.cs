using Asp.FireStore.Models;
using Asp.FireStore.Repository.Implementation;
using Asp.FireStore.Repository.IRepository;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.FireStore.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class RequestMechanicController : Controller
    {
        private readonly IRequestMechanicRepository _requestMechanicRepository;
        private readonly IUsersRepository _usersRepository;

        public RequestMechanicController(IRequestMechanicRepository requestMechanicRepository, IUsersRepository usersRepository)
        {
            _requestMechanicRepository = requestMechanicRepository;
            _usersRepository = usersRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _requestMechanicRepository.GetAllAsync("MechanicRequests");
            return View(res);
        }


        [HttpGet]
        public async Task<IActionResult> Print()
        {
            var res = await _requestMechanicRepository.GetAllAsync("MechanicRequests");
            return View(res);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RequestMechanic mechanic, string id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res = await _usersRepository.GetById("Users", id);

            string[] targetLocation = Request.Form["currentLocation"].ToString().Split(',');
            mechanic.CurrentLocation = new GeoPoint(Math.Round(double.Parse(targetLocation[0]), 5), Math.Round(double.Parse(targetLocation[1]), 5));

            mechanic.UserId = id;
            mechanic.Status = 0;
            mechanic.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            mechanic.Stander.CreatedBy = AdminId;

            mechanic.Stander.IsActive = true;
            mechanic.Stander.IsDelete = false;
            await _requestMechanicRepository.AddAsync("MechanicRequests", mechanic);
            return RedirectToAction("Profile",$"{res.UserType}", new {id = mechanic.UserId});
        }

        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            var data = await _requestMechanicRepository.GetById("MechanicRequests", id);
            data.Stander.IsActive = true;
            await _requestMechanicRepository.Update("MechanicRequests", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DisActive(string id)
        {
            var data = await _requestMechanicRepository.GetById("MechanicRequests", id);
            data.Stander.IsActive = false;
            await _requestMechanicRepository.Update("MechanicRequests", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var res = await _requestMechanicRepository.GetById("MechanicRequests", id);
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Accept(string id, string mechanicid)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res = await _requestMechanicRepository.GetById("MechanicRequests", id);
            res.MechanicId = mechanicid;
            res.Status = 1;
            res.Stander.ModifiedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            await _requestMechanicRepository.Update("MechanicRequests", id, res);
            return RedirectToAction("Profile", "Mechanic", new { id = mechanicid });

        }


        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            var data = await _requestMechanicRepository.GetById("MechanicRequests", id);
            data.Stander.IsDelete = true;
            data.Stander.DeletedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.DeletedBy = AdminId;

            await _requestMechanicRepository.Update("MechanicRequests", id, data);
            return RedirectToAction("Index");
        }

    }
}
