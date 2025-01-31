﻿using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Application.Errors;
using Persistance;

namespace Application.ABot
{
    public class Details
    {
        public class Query : IRequest<BotFrontend>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, BotFrontend>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<BotFrontend> Handle(Query request, CancellationToken cancellationToken)
            {
                Bot bot = await _context.Bot.FindAsync(request.Id);
                if (bot == null)
                    throw new RestException(HttpStatusCode.NotFound, new
                    {
                        settings = "Could Not find bot"
                    });

                
                BotFrontend botFrontend = new BotFrontend();
                botFrontend.Id = bot.Id;
                botFrontend.Nick = bot.Nick;
                botFrontend.Description = bot.Description;
                botFrontend.Type = bot.Type;
                botFrontend.ImageUrl = bot.ImageUrl;
                return botFrontend;
            }
        }
    }
}
