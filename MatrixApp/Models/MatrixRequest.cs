namespace MatrixApp.Models
{
    public class MatrixRequest
    {
        public double[][] Matrix { get; set; } = Array.Empty<double[]>();
    }

    public class MatrixResponse
    {
        public double Average { get; set; }
        public double[][] Matrix { get; set; } = Array.Empty<double[]>();
        public string Message { get; set; } = string.Empty;
    }

    public class GenerateRequest
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}