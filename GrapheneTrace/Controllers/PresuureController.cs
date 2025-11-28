using GrapheneTrace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GrapheneTrace.Controllers
{
    public class PressureController : Controller
    {
        public static int[,] Matrix { get; set; } = new int[32, 32];

        // NEW: store last uploads (optional)
        public static List<PressureRecord> Records { get; set; } = new List<PressureRecord>();

        public IActionResult ShowMatrix()
        {
            ViewBag.Matrix = Matrix;


            // if there is a last record, send metrics to the view
            if (Records.Any())
            {
                var last = Records.Last();
                ViewBag.PeakPressure = last.PeakPressure;
                ViewBag.ContactAreaPercent = last.ContactAreaPercent;
            }

            return View();
        }
    }
}