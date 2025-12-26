using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using TimeSeriesRoot.Application.TimeSeries.Responses;
using TimeSeriesRoot.Application.TimeSeries.Commands;
using TimeSeriesRoot.Application.TimeSeries.Queries;
using TimeSeriesRoot.Domain.Enums;
using Xunit.Sdk;

namespace TimeSeriesRoot.Api.Controllers
{
    [ApiController]
    [Route("api/timeseries")]
    public class TimeSeriesQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TimeSeriesQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetTimeSeriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimeSeries(int page, int pageSize)
        {
            try
            {
                var query = new GetTimeSeriesQuery { Page = page, PageSize = pageSize };
                GetTimeSeriesResponse result = await _mediator.Send(query, HttpContext.RequestAborted);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Validation failed in GetTimeSeries: {ex.Message}\n{ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet("{id}/data")]
        [ProducesResponseType(typeof(GetTimeSeriesByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimeSeriesById(int id, string unit, string start, string end)
        {
            try
            {
                if (!Enum.TryParse<EnergyUnit>(unit, ignoreCase: true, out var parsedUnit))
                {
                    throw new ValidationException($"Invalid status '{unit}'. Allowed values: {string.Join(", ", Enum.GetNames<EnergyUnit>())}");
                }

                var query = new GetTimeSeriesByIdQuery { Id = id, Unit = parsedUnit,  Start = start, End = end };
                GetTimeSeriesByIdResponse result = await _mediator.Send(query, HttpContext.RequestAborted);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Validation failed in GetTimeSeriesById: {ex.Message}\n{ex.StackTrace}");
                return BadRequest();
            }
        }
        [HttpGet("{id}/kpi")]
        [ProducesResponseType(typeof(GetTimeSeriesKpiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimeSeriesKpi(int id, string periodStart, string periodEnd)
        {
            try
            {
                var query = new GetTimeSeriesKpiQuery { Id = id, PeriodStart = periodStart, PeriodEnd = periodEnd };
                GetTimeSeriesKpiResponse result = await _mediator.Send(query, HttpContext.RequestAborted);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Validation failed in GetTimeSeriesKpi: {ex.Message}\n{ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
