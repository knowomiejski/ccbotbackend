using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.ABot
{
    public class List
    {
        public class Query : IRequest<List<BotFrontend>>
        {

        }

        public class Handler : IRequestHandler<Query, List<BotFrontend>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<BotFrontend>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Bot> botList = await _context.Bot.ToListAsync();
                List<BotFrontend> botFrontendList = new List<BotFrontend>();
                foreach(Bot bot in botList)
                {
                    BotFrontend newBotFrontend = new BotFrontend();
                    newBotFrontend.Id = bot.Id;
                    newBotFrontend.Nick = bot.Nick;
                    newBotFrontend.Description = bot.Description;
                    newBotFrontend.ImageUrl = bot.ImageUrl;
                    botFrontendList.Add(newBotFrontend);
                }
                return botFrontendList;
            }
        }
    }
}
