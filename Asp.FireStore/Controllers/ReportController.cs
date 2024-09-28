using Asp.FireStore.Models;
using Asp.FireStore.Models.ViewModel;
using Asp.FireStore.Repository.Implementation;
using Asp.FireStore.Repository.IRepository;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.FireStore.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ReportController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IReportRepository _reportRepository;

        public ReportController(IUsersRepository usersRepository, IReportRepository reportRepository)
        {
            _usersRepository = usersRepository;
            _reportRepository = reportRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _reportRepository.GetAllAsync("Reports");
            return View();
        }

        [HttpGet]
        public IActionResult Add(string id, string Uid)
        {
            return View(new ReportVM { Id = id , UId = Uid });
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(ReportVM report)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            report.Reports.UserId = report.UId;
            report.Reports.RequestId = report.Id;
            report.Reports.Status = 0;
            report.Reports.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            report.Reports.Stander.CreatedBy = AdminId;

            report.Reports.Stander.IsActive = true;

            await _reportRepository.AddAsync("Reports", report.Reports);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Read(string id)
        {
            var res = await _reportRepository.GetById("Reports", id);
            res.Status = 1;
            res.Stander.ReadAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            res.Stander.ReadBy = AdminId;

            await _reportRepository.Update("Reports",id,res);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var res = await _reportRepository.GetById("Reports", id);
            return View(res);
        }

    }
}
