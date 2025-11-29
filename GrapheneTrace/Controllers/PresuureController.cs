using GrapheneTrace.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GrapheneTrace.Controllers
{
    public class PressureController : Controller
    {
        // Latest uploaded matrix
        public static int[,] Matrix { get; set; } = new int[0, 0];

        // History of calculated metrics
        public static List<PressureRecord> Records { get; set; } = new List<PressureRecord>();

        public IActionResult ShowMatrix()
        {
            int rows = Matrix.GetLength(0);
            int cols = Matrix.GetLength(1);

            ViewBag.Matrix = Matrix;
            ViewBag.Rows = rows;
            ViewBag.Cols = cols;

            if (rows == 0 || cols == 0)
            {
                ViewBag.PeakPressure = 0;
                ViewBag.ContactAreaPercent = 0;
                ViewBag.AveragePressure = 0;
                ViewBag.Alert = false;
                return View();
            }

            int peak = 0;
            int totalPressure = 0;
            int totalPixels = rows * cols;
            int pixelsAboveThreshold = 0;

            int lowerThreshold = 10;    // for Contact Area %
            int alertThreshold = 150;   // if any cell > this => alert

            bool alert = false;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int value = Matrix[i, j];

                    // peak
                    if (value > peak)
                        peak = value;

                    // contact area
                    if (value >= lowerThreshold)
                        pixelsAboveThreshold++;

                    // sum for average
                    totalPressure += value;

                    // alert check
                    if (value > alertThreshold)
                        alert = true;
                }
            }

            double contactPercent = totalPixels > 0
                ? (pixelsAboveThreshold * 100.0 / totalPixels)
                : 0.0;

            double averagePressure = totalPixels > 0
                ? (totalPressure * 1.0 / totalPixels)
                : 0.0;

            ViewBag.PeakPressure = peak;
            ViewBag.ContactAreaPercent = Math.Round(contactPercent, 1);
            ViewBag.AveragePressure = Math.Round(averagePressure, 1);
            ViewBag.Alert = alert;

            // Save record in history
            Records.Add(new PressureRecord
            {
                PeakPressure = peak,
                ContactAreaPercent = contactPercent,
                AveragePressure = averagePressure,
                Date = DateTime.Now.ToString("dd MMM yyyy HH:mm"),
                Alert = alert
            });

            return View();
        }

        public IActionResult Reports()
        {
            ViewBag.Records = Records;
            return View();
        }
    }
}