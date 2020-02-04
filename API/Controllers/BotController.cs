using Application.ABot;
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
    public class BotController : ControllerBase


    {
        private readonly IMediator _mediator;
        public BotController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/bot
        [HttpGet]
        public async Task<ActionResult<List<BotFrontend>>> List()
        {
            return await _mediator.Send(new List.Query());
        }

        // GET api/bot/id
        [HttpGet("{id}")]
        public async Task<ActionResult<BotFrontend>> Details(Guid id)
        {
            return await _mediator.Send(new Details.Query { Id = id });
        }

        // POST api/bot
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await _mediator.Send(command);
        }

        // PUT api/bot/id
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command, Guid id)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }
        // DELETE api/bot
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            
            return await _mediator.Send(new Delete.Command { Id = id });
        }


    }
}
