using MediatR;

using Microsoft.AspNetCore.Mvc;

using Phonebook.Report.Application.Commands.Report;
using Phonebook.Report.Application.Models.Requests.Report;
using Phonebook.Report.Application.Queries;

namespace Phonebook.Report.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {

        private readonly IMediator mediator;

        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateReportRequestModel request)
        {
            var result = await mediator.Send(new CreateReportCommand(request));
            return Ok(result);
        }
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var result = await mediator.Send(new ReportListQuery());
            return Ok(result);
        }

        [HttpGet("Details/{Id}")]
        public async Task<IActionResult> Details([FromRoute] ReportDetailsRequestModel request)
        {
            var result = await mediator.Send(new ReportDetailsQuery(request));
            return Ok(result);
        }
    }
}
