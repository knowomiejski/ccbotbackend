using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace API.Services
{
    public class QuestionsBotService : BackgroundService
    {
        private IQuestionsBotQueue _queue;
        
        public QuestionsBotService(IQuestionsBotQueue queue)
        {
            _queue = queue;
            Console.Out.WriteLine("Starting service");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await _queue.PopQueue(stoppingToken);

                await task(stoppingToken); 
            }
        }
    }
}