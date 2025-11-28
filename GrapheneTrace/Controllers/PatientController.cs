using GrapheneTrace.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    public class PatientController : Controller
{
        
        public static List<Patient> Patients = new List<Patient>();


        

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Patient p)
    {
        Patients.Add(p);
        return RedirectToAction("List");
    }

    public IActionResult List()
    {
        return View(Patients);
    }
}
}
