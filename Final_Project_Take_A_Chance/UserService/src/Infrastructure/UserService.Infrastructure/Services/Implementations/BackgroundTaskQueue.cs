using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using UserService.Application.Services.Interfaces;

namespace UserService.Infrastructure.Services.Implementations
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue;

        public BackgroundTaskQueue(int capacity = 100)
        {
            _queue = Channel.CreateBounded<Func<CancellationToken, Task>>(capacity);
        }
        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (!_queue.Writer.TryWrite(workItem))
            {
                throw new InvalidOperationException("Task queue is full.");
            }
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);
            return workItem;
        }
    }
}
