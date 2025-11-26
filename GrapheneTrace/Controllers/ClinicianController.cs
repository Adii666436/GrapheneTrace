using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GrapheneTrace.Controllers
{
    public class ClinicianController : Controller
    {
        // Store upload history (optional – for dashboard)
        public static List<dynamic> Uploads = new List<dynamic>();

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ViewBag.Error = "Please select a CSV file.";
                return View();
            }

            using var reader = new StreamReader(csvFile.OpenReadStream());
            string fileContent = reader.ReadToEnd().Trim();

            // Clean and split correctly
            var lines = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            int rowCount = lines.Length;
            int colCount = lines[0].Split(',').Length;

            // Create matrix with correct size
            int[,] matrix = new int[rowCount, colCount];

            // Safe parsing
            for (int i = 0; i < rowCount; i++)
            {
                var rowValues = lines[i].Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < colCount; j++)
                {
                    matrix[i, j] = int.Parse(rowValues[j]);
                }
            }

            // Save matrix globally
            PressureController.Matrix = matrix;

            // Save upload record
            Uploads.Add(new
            {
                FileName = csvFile.FileName,
                Date = DateTime.Now.ToString("dd MMM yyyy HH:mm"),
                Rows = rowCount,
                Cols = colCount
            });

            return RedirectToAction("ShowMatrix", "Pressure");
        }
    }
}