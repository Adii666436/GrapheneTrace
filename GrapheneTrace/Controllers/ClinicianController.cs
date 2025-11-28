using GrapheneTrace.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GrapheneTrace.Controllers
{
    public class ClinicianController : Controller
    {
        // Store upload history
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

            PressureController.Matrix = matrix;


            // Safe parsing
            for (int i = 0; i < rowCount; i++)
            {
                var rowValues = lines[i].Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < colCount; j++)
                {
                    matrix[i, j] = int.Parse(rowValues[j]);
                }
            }


            // ---- Compute Metrics ----
            int peakPressure = matrix.Cast<int>().Max();

            int threshold = 40;
            int totalPixels = rowCount * colCount;
            int activePixels = matrix.Cast<int>().Count(v => v > threshold);
            double contactAreaPercent = Math.Round((activePixels * 100.0) / totalPixels, 2);

            // Average Pressure
            double averagePressure = Math.Round(matrix.Cast<int>().Average(), 2);

            // Save metrics to PressureController
            PressureController.Records.Add(new PressureRecord
            {
                PeakPressure = peakPressure,
                ContactAreaPercent = contactAreaPercent,
                AveragePressure = averagePressure,
                Date = DateTime.Now.ToString("dd MMM yyyy HH:mm"),
                Matrix = matrix
            });


           
            // Alert cjeck

            int alertThreshold = 40;
            bool alert = false;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (matrix[i, j] > alertThreshold)
                    {
                        alert = true;
                        break;
                    }
                }
                if (alert) break;
            }



            // Save upload record
            Uploads.Add(new
            {
                FileName = csvFile.FileName,
                Date = DateTime.Now.ToString("dd MMM yyyy HH:mm"),
                Rows = rowCount,
                Cols = colCount,
                Alert = alert
            });

            return RedirectToAction("ShowMatrix", "Pressure");
        }
    }
}