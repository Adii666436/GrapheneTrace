using Microsoft.AspNetCore.Mvc;
using GrapheneTrace.Models;
using System.Linq;
using GrapheneTrace.Controllers;   // <-- Needed so Dashboard can access ClinicianController.Uploads

namespace GrapheneTrace.Controllers
{
    public class UserController : Controller
    {
        public static List<User> Users = new List<User>();

        // LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN POST
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // Send Username + Role
            return RedirectToAction("Dashboard", new { username = user.Username, role = user.Role });
        }

        // REGISTER PAGE
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER POST
        [HttpPost]
        public IActionResult Register(User user)
        {
            Users.Add(user);
            return RedirectToAction("Login");
        }

        // DASHBOARD
        public IActionResult Dashboard(string username, string role)
        {
            ViewBag.Username = username;
            ViewBag.Role = role;

            // Admin summary
            ViewBag.PatientCount = PatientController.Patients.Count;
            ViewBag.ClinicianCount = UserController.Users.Count(u => u.Role == "Clinician");
            ViewBag.UploadCount = ClinicianController.Uploads.Count;

            // Clinician summary
            ViewBag.AlertCount = ClinicianController.Uploads.Count(u => u.Alert == true);
            ViewBag.ReportCount = 0;

            // Clinician recent uploads
            ViewBag.RecentUploads = ClinicianController.Uploads.TakeLast(5).ToList();


            // Patient summary
            if (ClinicianController.Uploads.Count > 0)
                ViewBag.LastUploadDate = ClinicianController.Uploads.Last().Date;

            return View();
        }
    }
}