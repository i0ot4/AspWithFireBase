using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Asp.FireStore.Repository.IRepository;
using Asp.FireStore.Models;
using Google.Cloud.Firestore;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Asp.FireStore.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository _usersRepository)
        {
            this._usersRepository = _usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _usersRepository.GetAllAsync("Users", x=> x.UserType != "Admin"  && x.UserType != "SuperAdmin");
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Print()
        {
            var res = await _usersRepository.GetAllAsync("Users", x=> x.UserType != "Admin"  && x.UserType != "SuperAdmin");
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var respo = await _usersRepository.GetById("Users",id);
            return View(respo);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Users users)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            users.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            users.Stander.CreatedBy = AdminId;

            users.Stander.IsActive = true;
            await _usersRepository.AddAsync("Users", users);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var data = await _usersRepository.GetById("Users", id);
            return View(data);
        }


        [HttpPut]
        public async Task<IActionResult> Edit(string id, IFormFile formFile)
        {
            var res = await _usersRepository.GetById("Users", id);
            if (formFile != null)
            {
                var uplaod = await _usersRepository.UploadImageAsync("images/profiles", formFile);
                res.ProfilePicture = uplaod;
            }
            res.Stander.ModifiedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            res.Stander.ModifiedBy = AdminId;

            await _usersRepository.Update("Users", id, res);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            var data = await _usersRepository.GetById("Users", id);
            data.Stander.IsDelete = true;
            data.Stander.DeletedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.DeletedBy = AdminId;

            await _usersRepository.Update("Users", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Restor(string id)
        {
            var data = await _usersRepository.GetById("Users", id);
            data.Stander.IsDelete = false;
            await _usersRepository.Update("Users", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            var data = await _usersRepository.GetById("Users", id);
            data.Stander.IsActive = true;
            await _usersRepository.Update("Users", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DisActive(string id)
        {
            var data = await _usersRepository.GetById("Users", id);
            data.Stander.IsActive = false;

            data.Stander.BlockedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.BlockedBy = AdminId;

            await _usersRepository.Update("Users", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string id)
        {
            var data = await _usersRepository.GetById("Users", id);
            data.Stander.IsConfirm = true;

            data.Stander.ConfirmAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.ConfirmBy = AdminId;

            await _usersRepository.Update("Users", id, data);
            if (data.UserType == "TaxiDriver")
            {
                return RedirectToAction("Index", "TaxiDriver");
            }
            else if (data.UserType == "TrafficPolice")
            {
                return RedirectToAction("Index", "TrafficPolice");
            }
            else if (data.UserType == "Mechanic")
            {
                return RedirectToAction("Index", "Mechanic");
            }
            return View();
        }

    }
}
