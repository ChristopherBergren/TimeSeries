using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using TimeSeries.Application.Commands;
using TimeSeries.Application.Queries;
using TimeSeries.Application.Responses;

namespace TimeSeries.Api.Controllers
{
    [ApiController]
    [Route("api/timeseries")]
    public class TimeSeriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TimeSeriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Import Time Series
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("parse")]
        [ProducesResponseType(typeof(UpsertTimeSeriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportTimeSeries(UpsertTimeSeriesCommand command)
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

        /// <summary>
        /// Bulk Import Time Series-files
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("collect")]
        [ProducesResponseType(typeof(UpsertTimeSeriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkImportTimeSeries(UpsertTimeSeriesCommand command)
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
    }
}
