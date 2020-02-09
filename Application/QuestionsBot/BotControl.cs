using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Persistance;

namespace Application.QuestionsBot
{
    public class BotControl
    {
        public class Command : IRequest
        {
            public string BotAction { get; set; }
            public Guid BotId { get; set; }
            public Guid SettingsId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.BotAction).NotEmpty();
                RuleFor(x => x.BotId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private ITwitchbot _twitchBot;

            public Handler(DataContext context, ITwitchbot twitchBot)
            {
                _context = context;
                _twitchBot = twitchBot;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var selectedBot = await _context.Bot.FindAsync(request.BotId);
                var selectedSettings = await _context.Settings.FindAsync(request.SettingsId);

                switch (request.BotAction)
                {
                    case "start":
                        HandleBotStart(selectedBot, selectedSettings);
                        break;
                    case "stop":
                        HandleBotStop();
                        break;
                }

                return Unit.Value;
            }

            private void HandleBotStart(Bot bot, Settings settings)
            {
                if (_twitchBot == null || !_twitchBot.IsRunning)
                {
                    _twitchBot.Bot = bot;
                    _twitchBot.Settings = settings;
                    _twitchBot.StartBot();
                    Console.WriteLine(_twitchBot.GetHashCode());
                }
            }

            private void HandleBotStop()
            {
                if (_twitchBot != null && _twitchBot.IsRunning)
                {
                    Console.WriteLine(_twitchBot.GetHashCode());
                    _twitchBot.StopBot();
                }
            }
        }
    }
}