using Xunit;
using MatrixApp.Services;

namespace MatrixApp.Tests
{
    public class MatrixServiceTests
    {
        private readonly MatrixService _service = new();

        [Fact]
        public void CalculateAverage_SimpleMatrix_ReturnsCorrectAverage()
        {
            var matrix = new double[][]
            {
                new double[] { 2, 4, 6 },
                new double[] { 8, 10, 12 }
            };

            var result = _service.CalculateAverage(matrix);

            Assert.Equal(7.0, result);
        }

        [Fact]
        public void CalculateAverage_SingleElement_ReturnsElement()
        {
            var matrix = new double[][] { new double[] { 5.5 } };
            Assert.Equal(5.5, _service.CalculateAverage(matrix));
        }

        [Fact]
        public void GenerateMatrix_CreatesCorrectDimensions()
        {
            var matrix = _service.GenerateMatrix(3, 4, 0, 100);

            Assert.Equal(3, matrix.Length);
            Assert.Equal(4, matrix[0].Length);
        }

        [Fact]
        public void CalculateAverage_EmptyMatrix_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                _service.CalculateAverage(new double[][] { }));
        }
    }
}