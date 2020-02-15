using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;

namespace API.Services
{
    public class QuestionsBotQueue : IQuestionsBotQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _tasks;
        private SemaphoreSlim _signal;
        
        public QuestionsBotQueue()
        {
            Console.Out.WriteLine("Creating Queue");
            _tasks = new ConcurrentQueue<Func<CancellationToken, Task>>();
            _signal = new SemaphoreSlim(0);
        }

        public void QueueTask(Func<CancellationToken, Task> task)
        {
            _tasks.Enqueue(task);
            _signal.Release();
        }

        public async Task<Func<CancellationToken, Task>> PopQueue(CancellationToken cancellationToken)
        {
            Console.Out.WriteLine("Waiting for task");
            await _signal.WaitAsync(cancellationToken);
            _tasks.TryDequeue(out var task);
            return task;
        }
    }
}