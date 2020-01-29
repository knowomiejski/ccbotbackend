using System;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Persistance;

namespace Application.ASettings
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Prefix { get; set; }
            public int? ReminderTimer { get; set; }
            public string FolderId { get; set; }
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
                    throw new Exception("These settings do not exist");

                settings.Name = request.Name ?? settings.Name;
                settings.Prefix = request.Prefix ?? settings.Prefix;
                settings.ReminderTimer = request.ReminderTimer ?? settings.ReminderTimer;
                settings.FolderId = request.FolderId ?? settings.FolderId;
                
                bool success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving settings changes");
            }
        }
    }
}
