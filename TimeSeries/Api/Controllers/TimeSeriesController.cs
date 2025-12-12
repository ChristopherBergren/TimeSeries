using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TimeSeries.Application.Queries;
using TimeSeries.Application.Commands;
using TimeSeries.Application.Responses;

namespace TimeSeries.Api.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class TimeSeriesController(IMediator mediator,
    IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IMediator _mediator = mediator;


        [HttpGet("parse")]
        [ProducesResponseType(typeof(GetTimeSeriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimeSeries(GetTimeSeriesQuery query, CancellationToken cancellationToken) 
        {
            try
            {
                GetTimeSeriesResponse result = await _mediator.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Exception in GetTimeSeries: {ex.Message}\n{ex.StackTrace}");
                return BadRequest();
            }
        }


        //[HttpPost("parse")]
        //public async Task<IActionResult> UploadCsv(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    using var stream = file.OpenReadStream();
        //    using var reader = new StreamReader(stream);

        //    // Read the first line (headers)
        //    var headerLine = await reader.ReadLineAsync();
        //    if (string.IsNullOrWhiteSpace(headerLine))
        //        return BadRequest("CSV file is empty or has no header.");

        //    // Split headers by comma (you may need more complex parsing for quoted commas)
        //    var headers = headerLine.Split(',').Select(h => h.Trim()).ToArray();

        //    return Ok(headers);
        //}
    }
}
