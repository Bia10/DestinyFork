using System;
using System.Collections.Generic;
using System.Threading;

namespace Destiny.Collections
{
    public class PendingKeyedQueue<TKey, TValue> : Dictionary<TKey, TValue>, IDisposable
    {
        private readonly ManualResetEvent QueueDone = new ManualResetEvent(false);

        public PendingKeyedQueue() : base() { }

        public void Enqueue(TKey key, TValue value)
        {
            Add(key, value);

            QueueDone.Set();
        }

        public TValue Dequeue(TKey key)
        {
            while (!ContainsKey(key))
            {
                QueueDone.WaitOne();
            }

            TValue value = this[key];

            Remove(key);

            QueueDone.Reset();

            return value;
        }

        public void Dispose()
        {
            QueueDone.Dispose();
        }
    }
}