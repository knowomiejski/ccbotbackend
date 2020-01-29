using Application.ASettings;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase


    {
        private readonly IMediator _mediator;
        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/settings
        [HttpGet]
        public async Task<ActionResult<List<Settings>>> List()
        {
            return await _mediator.Send(new List.Query());
        }

        // GET api/settings/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Settings>> Details(Guid id)
        {
            return await _mediator.Send(new Details.Query { Id = id });
        }

        // POST api/settings
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await _mediator.Send(command);
        }

        // PUT api/settings/id
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command, Guid id)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }
        // DELETE api/settings
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            
            return await _mediator.Send(new Delete.Command { Id = id });
        }


    }
}
