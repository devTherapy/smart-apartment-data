using MediatR;
using Microsoft.AspNetCore.Mvc;
using Smart_Data.Application.Dtos;
using System.Threading.Tasks;

namespace Smart_Data.API.Controllers
{
    [ApiController]
    [Route("api/smart-data")]
    public class SmartData : ControllerBase
    {
        private readonly IMediator _mediator;

        public SmartData(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("search")]
        public async Task<ActionResult<object>> Search([FromQuery] SearchQueryParams parameters)
        {
            return await _mediator.Send(parameters);
        }
    }
}
