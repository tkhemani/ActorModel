using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorImpl
{
    class QueueActor<T>
    {
        private readonly ActorSyncContext _msgQueue = new ActorSyncContext();

        private readonly Queue<T> _items = new Queue<T>();

        public async Task enqueueAsync(T item)
        {
            await _msgQueue;
            _items.Enqueue(item);
        }

        public async Task EnqueueAsync(T item)
        {
            // (note: we need to implement a custom awaiter to allow awaiting the message queue)
            await _messageQueue;
            _items.Enqueue(item);
        }

        // (note: using my option type (see post) for the result)
        public async Task<May<T>> TryDequeueAsync()
        {
            await _messageQueue;
            if (_items.Count == 0) return May.NoValue;
            return _items.Dequeue();
        }

        // the async keyword allows using await
        // we return a task, instead of void, so callers can await us finishing
        async Task UseQueue()
        {
            var q = new QueueActor<int>();

            // sending messages and waiting for the responses
            await q.EnqueueAsync(1);
            May<int> r1 = await q.TryDequeueAsync(); // r1 will contain 1
            May<int> r2 = await q.TryDequeueAsync(); // r2 will contain no value

            // spamming messages, then later checking for the responses
            Task t3 = q.EnqueueAsync(2);
            Task<May<int>> t4 = q.TryDequeueAsync();
            Task<May<int>> t5 = q.TryDequeueAsync();
            await t3; // if our enqueue had failed somehow, this would rethrow the exception
            var r5 = await t5; // r5 will contain no value
            var r4 = await t4; // r4 will contain 2
        }


    }
}


//ref :http://twistedoakstudios.com/blog/Post2061_emulating-actors-in-c-with-asyncawait