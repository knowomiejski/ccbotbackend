using Application.ABot;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BotController : BaseController


    {

        // GET api/bot
        [HttpGet]
        public async Task<ActionResult<List<BotFrontend>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        // GET api/bot/id
        [HttpGet("{id}")]
        public async Task<ActionResult<BotFrontend>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        // POST api/bot
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        // PUT api/bot/id
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command, Guid id)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
        // DELETE api/bot
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            
            return await Mediator.Send(new Delete.Command { Id = id });
        }


    }
}
