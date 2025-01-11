﻿using MediatR;

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

        private readonly IMediator mediator;

        public PersonController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AddPersonRequestModel request)
        {
            var result = await mediator.Send(new AddPersonCommand(request));
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonRequestModel request)
        {
            var result = await mediator.Send(new UpdatePersonCommand(request));
            return Ok(result);
        }
    }
}
