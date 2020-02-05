using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Application.Errors;
using Persistance;

namespace Application.ASettings
{
    public class Details
    {
        public class Query : IRequest<Settings>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Settings>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Settings> Handle(Query request, CancellationToken cancellationToken)
            {
                Settings settings = await _context.Settings.FindAsync(request.Id);
                
                if (settings == null)
                    throw new RestException(HttpStatusCode.NotFound, new
                    {
                        settings = "Could Not find settings"
                    });
                
                return settings;
            }
        }
    }
}
