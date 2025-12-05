namespace MatrixApp.Services
{
    public interface IMatrixService
    {
        double CalculateAverage(double[][] matrix);
        double[][] GenerateMatrix(int rows, int cols, double min, double max);
    }

    public class MatrixService : IMatrixService
    {
        public double CalculateAverage(double[][] matrix)
        {
            if (matrix == null || matrix.Length == 0)
                throw new ArgumentException("Matrix cannot be empty");

            double sum = 0;
            int count = 0;

            foreach (var row in matrix)
            {
                foreach (var element in row)
                {
                    sum += element;
                    count++;
                }
            }

            return count > 0 ? sum / count : 0;
        }

        public double[][] GenerateMatrix(int rows, int cols, double min, double max)
        {
            var random = new Random();
            var matrix = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                matrix[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    matrix[i][j] = Math.Round(random.NextDouble() * (max - min) + min, 0);
                }
            }

            return matrix;
        }
    }
}
