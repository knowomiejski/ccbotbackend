using System;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Persistance;

namespace Application.ABot
{
    public class Create
    {
        public class Command : IRequest
        {
            public string TMIToken { get; set; }
            public string TwitchClientId { get; set; }
            public string Nick { get; set; }
            public string Description { get; set; }
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
                Bot bot = new Bot
                {
                    Id = Guid.NewGuid(),
                    TMIToken = request.TMIToken,
                    TwitchClientId = request.TwitchClientId,
                    Nick = request.Nick,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl
                };

                _context.Bot.Add(bot);
                bool success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving settings changes");
            }
        }
    }
}
