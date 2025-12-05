using Microsoft.AspNetCore.Mvc;
using MatrixApp.Models;
using MatrixApp.Services;

namespace MatrixApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatrixController : ControllerBase
    {
        private readonly IMatrixService _matrixService;

        public MatrixController(IMatrixService matrixService)
        {
            _matrixService = matrixService;
        }

        [HttpPost("calculate")]
        public IActionResult CalculateAverage([FromBody] MatrixRequest request)
        {
            try
            {
                var average = _matrixService.CalculateAverage(request.Matrix);
                return Ok(new MatrixResponse 
                { 
                    Average = average,
                    Matrix = request.Matrix,
                    Message = $"Среднее арифметическое: {average:F2}"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("generate")]
        public IActionResult GenerateMatrix([FromBody] GenerateRequest request)
        {
            try
            {
                var matrix = _matrixService.GenerateMatrix(request.Rows, request.Cols, request.Min, request.Max);
                var average = _matrixService.CalculateAverage(matrix);
                
                return Ok(new MatrixResponse 
                { 
                    Average = average,
                    Matrix = matrix,
                    Message = $"Матрица {request.Rows}x{request.Cols} сгенерирована"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}