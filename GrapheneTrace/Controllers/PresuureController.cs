using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    public class PressureController : Controller
    {
        public static int[,] Matrix = new int[32, 32];

        public IActionResult ShowMatrix()
        {
            ViewBag.Matrix = Matrix;
            return View();
        }
    }
}
