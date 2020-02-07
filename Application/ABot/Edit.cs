using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Application.Errors;
using Persistance;

namespace Application.ABot
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string TMIToken { get; set; }
            public string TwitchClientId { get; set; }
            public string Nick { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public string ImageUrl { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Bot bot = await _context.Bot.FindAsync(request.Id);
                if (bot == null)
                    throw new RestException(HttpStatusCode.NotFound, new
                    {
                        settings = "Could Not find bot"
                    });

                bot.TMIToken = request.TMIToken ?? bot.TMIToken;
                bot.TwitchClientId = request.TwitchClientId ?? bot.TwitchClientId;
                bot.Nick = request.Nick ?? bot.Nick;
                bot.Description = request.Description ?? bot.Description;
                bot.Type = request.Type ?? bot.Type;
                bot.ImageUrl = request.ImageUrl ?? bot.ImageUrl;

                bool success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving settings changes");
            }
        }
    }
}