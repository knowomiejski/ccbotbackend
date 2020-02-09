using System.Threading.Tasks;
using Application.QuestionsBot;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class QuestionsBotController : BaseController
    {
        // GET api/questionsbot
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Unit>> Start(BotControl.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}