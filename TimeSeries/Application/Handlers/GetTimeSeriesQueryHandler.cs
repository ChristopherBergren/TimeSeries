using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using TimeSeries.Application.Models;
using TimeSeries.Application.Queries;
using TimeSeries.Application.Responses;

namespace TimeSeries.Application.Handlers
{
    internal class GetTimeSeriesQueryHandler
        : IRequestHandler<GetTimeSeriesQuery, GetTimeSeriesResponse>
    {
      

        public GetTimeSeriesQueryHandler() {
           
        }

        public async Task<GetTimeSeriesResponse> Handle(GetTimeSeriesQuery query, CancellationToken cancellationToken)
        {
            


            var response = new GetTimeSeriesResponse();

            return response;
        }
    }
}