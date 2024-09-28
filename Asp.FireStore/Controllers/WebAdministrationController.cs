using Asp.FireStore.Models;
using Asp.FireStore.Models.FirebaseModel;
using Asp.FireStore.Repository.Implementation;
using Asp.FireStore.Repository.IRepository;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Asp.FireStore.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class WebAdministrationController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public WebAdministrationController(IUsersRepository _usersRepository)
        {
            this._usersRepository = _usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _usersRepository.GetAllAsync("Users", x => x.UserType == "Admin" || x.UserType == "SuperAdmin");
            return View(res);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Users users, IFormFile formFile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var uplaod = await _usersRepository.UploadImageAsync("images/profiles", formFile);
            users.ProfilePicture = uplaod;
            users.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            users.Stander.CreatedBy = AdminId;

            users.Stander.IsActive = true;
            var res = await _usersRepository.SignUp(users.Email, users.Password);
            await _usersRepository.AddAsyncWithSignUp("Users", users, res);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var data = await _usersRepository.GetById("Users", id);
            return View(data);
        }


        [HttpPost]
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
            await _usersRepository.Update("Users", id, data);
            return RedirectToAction("Index");
        }
    }
}
