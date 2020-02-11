using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace API.Services
{
    public class QuestionsBotService : BackgroundService
    {
        private IQuestionsBotQueue _queue;
        
        public QuestionsBotService(IQuestionsBotQueue queue)
        {
            
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