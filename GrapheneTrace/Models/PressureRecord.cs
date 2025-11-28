namespace GrapheneTrace.Models
{
    public class PressureRecord
    {
        public int id { get; set; }

        public string patientName { get; set; } = string.Empty;


        public int[,] Matrix { get; set; } = new int[0, 0];

        public int PeakPressure { get; set; }
        public double ContactAreaPercent { get; set; }

        public double AveragePressure { get; set; }

        public string Date { get; set; } = string.Empty;

        public bool Alert { get; set; }
    }
}
