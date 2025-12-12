using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TimeSeries.Application.Queries;
using TimeSeries.Application.Commands;
using TimeSeries.Application.Responses;
using System.ComponentModel.DataAnnotations;

namespace TimeSeries.Api.Controllers
{
    [ApiController]
    [Route("api/timeseries")]
    public class TimeSeriesController(IMediator mediator,
    IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IMediator _mediator = mediator;

        [HttpPost("parse")]
        [ProducesResponseType(typeof(UpsertTimeSeriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimeSeries(UpsertTimeSeriesCommand command)
        {
            try
            {
                UpsertTimeSeriesResponse result = await _mediator.Send(command, HttpContext.RequestAborted);
                return Ok(result);
            }
            catch (FluentValidation.ValidationException ex)
            {
                Log.Error(ex, $"Validation failed in GetTimeSeries: {ex.Message}\n{ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetTimeSeriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimeSeries(GetTimeSeriesQuery query) 
        {
            try
            {
                GetTimeSeriesResponse result = await _mediator.Send(query, HttpContext.RequestAborted);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                Log.Error(ex, $"Validation failed in GetTimeSeries: {ex.Message}\n{ex.StackTrace}");
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
