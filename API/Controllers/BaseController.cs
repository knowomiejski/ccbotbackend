using API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;
        // private IQuestionsBotQueue _questionsBotQueue;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        // protected IQuestionsBotQueue QuestionsBotQueue => _questionsBotQueue ??= HttpContext.RequestServices.GetService<IQuestionsBotQueue>();
    }
}