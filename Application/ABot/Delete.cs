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
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
   
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

                _context.Remove(bot);
                
                bool success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving settings changes");
            }
        }
    }
}
