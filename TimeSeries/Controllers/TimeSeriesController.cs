using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace TimeSeries.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class TimeSeriesController : ControllerBase
    {
        public TimeSeriesController()
        {
        }

        [HttpPost("parse")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            // Read the first line (headers)
            var headerLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(headerLine))
                return BadRequest("CSV file is empty or has no header.");

            // Split headers by comma (you may need more complex parsing for quoted commas)
            var headers = headerLine.Split(',').Select(h => h.Trim()).ToArray();

            return Ok(headers);
        }
    }
}
