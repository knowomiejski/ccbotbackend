using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.ABot
{
    public class ListBotsOfType
    {
        public class Query : IRequest<List<BotFrontend>>
        {
            public string BotType { get; set; }
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
                var botList = await _context.Bot.ToListAsync();
                var botFrontendList = new List<BotFrontend>();
                foreach (var bot in botList)
                {
                    if (request.BotType == bot.Type)
                    {
                        var newBotFrontend = new BotFrontend
                        {
                            Id = bot.Id,
                            Nick = bot.Nick,
                            Description = bot.Description,
                            Type = bot.Type,
                            ImageUrl = bot.ImageUrl
                        };
                        botFrontendList.Add(newBotFrontend);
                    }
                }

                return botFrontendList;
            }
        }
    }
}