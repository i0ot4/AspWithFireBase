﻿using Asp.FireStore.Models;
using Asp.FireStore.Repository.Implementation;
using Asp.FireStore.Repository.IRepository;
using Firebase.Auth;
using Firebase.Auth.Repository;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asp.FireStore.Controllers
{

    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CustomerController : Controller
    {
        private readonly IUsersRepository usersRepository;

        public CustomerController(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await usersRepository.GetAllAsync("Users", x => x.UserType == "Customer");
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Print()
        {
            var res = await usersRepository.GetAllAsync("Users", x => x.UserType == "Customer");
            return View(res);
        }


        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var respo = await usersRepository.GetById("Users", id);
            return View(respo);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Users users , IFormFile formFile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res = await usersRepository.SignUp(users.Email, users.Password);

            var uplaod = await usersRepository.UploadImageAsync("images/profiles",formFile);

            users.ProfilePicture = uplaod;
            users.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            users.Stander.CreatedBy = AdminId;
            users.Stander.IsActive = true;

            await usersRepository.AddAsyncWithSignUp("Users", users, res);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var data = await usersRepository.GetById("Users", id);
            return View(data);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, IFormFile formFile)
        {
            var res = await usersRepository.GetById("Users", id);
            if (formFile != null)
            {
                var uplaod = await usersRepository.UploadImageAsync("images/profiles", formFile);
                res.ProfilePicture = uplaod;
            }
            res.Stander.ModifiedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            var AdminId = User.FindFirst("UserId")?.Value;
            res.Stander.ModifiedBy = AdminId;
            await usersRepository.Update("Users", id, res);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var res = await usersRepository.GetById("Users", id);
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            var data = await usersRepository.GetById("Users", id);
            data.Stander.IsActive = true;
            await usersRepository.Update("Users", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DisActive(string id)
        {
            var data = await usersRepository.GetById("Users", id);
            data.Stander.IsActive = false;

            data.Stander.BlockedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.BlockedBy = AdminId;

            await usersRepository.Update("Users", id, data);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
                var data = await usersRepository.GetById("Users", id);
                data.Stander.IsDelete = true;
                data.Stander.DeletedAt = Timestamp.FromDateTime(DateTime.UtcNow);
                var AdminId = User.FindFirst("UserId")?.Value;
                data.Stander.DeletedBy = AdminId;
                await usersRepository.Update("Users", id, data);
                return RedirectToAction("Index");
        }


    }
}