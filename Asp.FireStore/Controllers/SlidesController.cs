using Asp.FireStore.Models;
using Asp.FireStore.Repository.IRepository;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.FireStore.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class SlidesController : Controller
    {
        private readonly ISlidesRepository _slidesRepository;

        public SlidesController(ISlidesRepository slidesRepository)
        {
            _slidesRepository = slidesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _slidesRepository.GetAllAsync("Slides");
            return View(res);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Slides slides, IFormFile formFile)
        {
            var uplaod = await _slidesRepository.UploadImageAsync("images/Slides", formFile);
            slides.ImageUrl = uplaod;

            slides.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            var AdminId = User.FindFirst("UserId")?.Value;
            slides.Stander.CreatedBy = AdminId;
            slides.Stander.IsActive = true;
            await _slidesRepository.AddAsync("Slides", slides);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            var res = await _slidesRepository.GetById("Slides", id);

            res.Stander.DeletedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            res.Stander.IsActive = false;
            var AdminId = User.FindFirst("UserId")?.Value;
            res.Stander.DeletedBy = AdminId;
            await _slidesRepository.Update("Slides", id, res);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> Restore(string id)
        {
            var res = await _slidesRepository.GetById("Slides", id);
            res.Stander.IsActive = true;
            await _slidesRepository.Update("Slides", id, res);
            return RedirectToAction("Index");
        }

    }
}
