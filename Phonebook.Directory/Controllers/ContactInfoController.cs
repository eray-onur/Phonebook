using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Phonebook.Directory.Application.Commands.ContactInfo;
using Phonebook.Directory.Application.Models.Requests;
using Phonebook.Directory.Application.Models.Requests.ContactInfo;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactInfoController : ControllerBase
    {

        private readonly IMediator mediator;

        public ContactInfoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AddContactInfoRequestModel request)
        {
            var result = await mediator.Send(new AddContactInfoCommand(request));
            return Ok(result);
        }
    }
}
