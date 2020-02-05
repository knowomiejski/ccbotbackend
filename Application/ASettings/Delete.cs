using System;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Persistance;
using Application.Errors;
using System.Net;

namespace Application.ASettings
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
                Settings settings = await _context.Settings.FindAsync(request.Id);
                if (settings == null)
                    throw new RestException(HttpStatusCode.NotFound, new
                    {
                        settings = "Could Not find settings"
                    });

                _context.Remove(settings);
                
                bool success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving settings changes");
            }
        }
    }
}
