using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ViewerLibrary.model
{
    public class SerialTurnDataModel : ITurnDataModel
    {
        private object LOCK = new object();
        private Queue<Turn> queue = new Queue<Turn>();

        private Semaphore semaphore = new Semaphore(0, int.MaxValue);

        private bool streamEnded = false;

        public void Add(Turn turn, bool last)
        {
            lock (LOCK)
            {
                queue.Enqueue(turn);
                semaphore.Release();
                streamEnded = last;
            }
        }

        public bool HasNext()
        {
            if (!streamEnded) {
                semaphore.WaitOne();
            }
            lock (LOCK)
            {
                return !streamEnded || queue.Count > 0;
            }
        }

        public Turn Next()
        {
            lock (LOCK)
            {
                return queue.Dequeue();
            }
        }
    }
}
