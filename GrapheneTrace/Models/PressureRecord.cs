namespace GrapheneTrace.Models
{
    public class PressureRecord
    {
        public int id { get; set; }

        public string patientName { get; set; } = string.Empty;


        public int[,] Matrix { get; set; } = new int[0, 0];


    }
}
