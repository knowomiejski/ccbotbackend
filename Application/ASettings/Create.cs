using System;
using System.Threading.Tasks;
using MediatR;
using Domain;
using System.Threading;
using Persistance;
using FluentValidation;

namespace Application.ASettings
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string TargetChannel { get; set; }
            public string Prefix { get; set; }
            public int ReminderTimer { get; set; }
            public string FolderId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Prefix).NotEmpty();
                RuleFor(x => x.ReminderTimer).NotEmpty();
                RuleFor(x => x.FolderId).NotEmpty().WithMessage("yo");
            }
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
                Settings settings = new Settings
                {
                    Id = request.Id,
                    Name = request.Name,
                    TargetChannel = request.TargetChannel,
                    Prefix = request.Prefix,
                    ReminderTimer = request.ReminderTimer,
                    FolderId = request.FolderId
                };

                _context.Settings.Add(settings);
                bool success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving settings changes");
            }
        }
    }
}
