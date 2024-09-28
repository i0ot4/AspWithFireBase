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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _employeeRepository.GetAllAsync("Employee");
            return View(res);
        }
        

        [HttpGet]
        public async Task<IActionResult> Print()
        {
            var res = await _employeeRepository.GetAllAsync("Employee");
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var res = await _employeeRepository.GetById("Employee", id);
            return View(res);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Employee employee, string id, IFormFile formFile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            employee.MechanicId = id;
            var uplaod = await _employeeRepository.UploadImageAsync("images/Employee",formFile);
            employee.ProfilePicture = uplaod;
            employee.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            employee.Stander.CreatedBy = AdminId;

            employee.Stander.IsActive = true;
            employee.Stander.IsDelete = false;
            await _employeeRepository.AddAsync("Employee", employee);
            return RedirectToAction("Profile", "Mechanic", new { id = employee.MechanicId });

        }

        [HttpGet]
        public IActionResult AddTo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTo(Employee employee, IFormFile formFile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var uplaod = await _employeeRepository.UploadImageAsync("images/Employee", formFile);
            employee.ProfilePicture = uplaod;
            employee.Stander.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            employee.Stander.CreatedBy = AdminId;

            employee.Stander.IsActive = true;
            employee.Stander.IsDelete = false;
            await _employeeRepository.AddAsync("Employee", employee);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            var data = await _employeeRepository.GetById("Employee", id);
            data.Stander.IsActive = true;
            await _employeeRepository.Update("Employee", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DisActive(string id)
        {
            var data = await _employeeRepository.GetById("Employee", id);
            data.Stander.IsActive = false;

            data.Stander.BlockedAt = Timestamp.FromDateTime(DateTime.UtcNow);
            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.BlockedBy = AdminId;

            await _employeeRepository.Update("Employee", id, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            var data = await _employeeRepository.GetById("Employee", id);
            data.Stander.IsDelete = true;
            data.Stander.DeletedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            var AdminId = User.FindFirst("UserId")?.Value;
            data.Stander.DeletedBy = AdminId;

            await _employeeRepository.Update("Employee", id, data);
            return RedirectToAction("Index");
        }


    }
}
