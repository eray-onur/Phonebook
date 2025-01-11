using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Phonebook.Directory.Application.Commands.Person;
using Phonebook.Directory.Application.Models.Requests;
using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> logger;
        private readonly PhonebookDbContext phonebookDbContext;
        private readonly IMediator mediator;

        public PersonController(IMediator mediator, ILogger<WeatherForecastController> logger, PhonebookDbContext phonebookDbContext)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.phonebookDbContext = phonebookDbContext;
        }

        [AllowAnonymous]
        [HttpPost("CreatePerson")]
        public async Task<IActionResult> Create([FromBody] AddPersonRequestModel request)
        {
            var result = await mediator.Send(new AddPersonCommand(request));
            return Ok(result);
        }
    }
}
