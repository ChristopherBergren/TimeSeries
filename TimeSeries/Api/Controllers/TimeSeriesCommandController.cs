using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TimeSeriesRoot.Application.TimeSeries.Responses;
using TimeSeriesRoot.Application.TimeSeries.Queries;
using TimeSeriesRoot.Domain.Enums;
using TimeSeriesRoot.Application.TimeSeries.Commands;

namespace TimeSeriesRoot.Api.Controllers
{
    [ApiController]
    [Route("api/timeseries")]
    public class TimeSeriesCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TimeSeriesCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Import Time Series
        /// </summary>
        /// <param name="command"></param> 
        /// <returns></returns>
        [HttpPost("parse")]
        [ProducesResponseType(typeof(ImportTimeSeriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportTimeSeries(ImportTimeSeriesCommand command)
        {
            try
            {
                ImportTimeSeriesResponse result = await _mediator.Send(command, HttpContext.RequestAborted);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Validation failed in ImportTimeSeries: {ex.Message}\n{ex.StackTrace}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Bulk Import Time Series-files
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("collect")]
        [ProducesResponseType(typeof(ImportTimeSeriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkImportTimeSeries()
        {
            try
            {
                ImportTimeSeriesResponse result = await _mediator.Send(new BulkImportTimeSeriesCommand(), HttpContext.RequestAborted);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Validation failed in BulkImportTimeSeries: {ex.Message}\n{ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
