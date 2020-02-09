using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Persistance;

namespace Application.QuestionsBot
{
    public class Stop
    {
        public class Command : IRequest
        {
            public string Action { get; set; }
            public Guid BotId { get; set; }
            public Guid Sett { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Action).NotEmpty();
                RuleFor(x => x.BotId).NotEmpty();
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
                if (request.Action == "start")
                {
                }

                return Unit.Value;
            }
        }

    }
}