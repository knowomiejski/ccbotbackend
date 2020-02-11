using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IQuestionsBotQueue
    {
        void QueueTask(Func<CancellationToken, Task> task);
        Task<Func<CancellationToken, Task>> PopQueue(CancellationToken cancellationToken);
    }

    public class QuestionsBotQueue : IQuestionsBotQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _tasks;
        private SemaphoreSlim _signal;
        
        public QuestionsBotQueue()
        {
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
            await _signal.WaitAsync(cancellationToken);
            _tasks.TryDequeue(out var task);
            return task;
        }
    }
}