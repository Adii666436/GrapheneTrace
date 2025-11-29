using System;



namespace GrapheneTrace.Models
{
    public class PressureRecord
    {
        

        public int PeakPressure { get; set; }
        public double ContactAreaPercent { get; set; }

        public double AveragePressure { get; set; }

        public string Date { get; set; } = string.Empty;

        public bool Alert { get; set; }
    }
}
