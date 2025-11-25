using Microsoft.AspNetCore.Mvc;
using GrapheneTrace.Models;
using System.Linq;

namespace GrapheneTrace.Controllers
{
    public class UserController : Controller
    {
        // Temporary in-memory user list (no database)
        public static List<User> Users = new List<User>();

        // GET: /User/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /User/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // Send the username to Dashboard
            return RedirectToAction("Dashboard", new { username = user.Username });
        }

        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            Users.Add(user);
            return RedirectToAction("Login");
        }

        // GET: /User/Dashboard
        public IActionResult Dashboard(string username)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return RedirectToAction("Login");

            ViewBag.Username = user.Username;
            ViewBag.Role = user.Role;


            return View();
        }
    }
}