using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.ASettings
{
    public class List
    {
        public class Query : IRequest<List<Settings>>
        {

        }

        public class Handler : IRequestHandler<Query, List<Settings>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Settings>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Settings> settingsList = await _context.Settings.ToListAsync();
                return settingsList;
            }
        }
    }
}
