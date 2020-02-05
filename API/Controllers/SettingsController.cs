using Application.ASettings;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class SettingsController : BaseController


    {

        // GET api/settings
        [HttpGet]
        public async Task<ActionResult<List<Settings>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        // GET api/settings/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Settings>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        // POST api/settings
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        // PUT api/settings/id
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command, Guid id)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
        // DELETE api/settings
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            
            return await Mediator.Send(new Delete.Command { Id = id });
        }


    }
}
