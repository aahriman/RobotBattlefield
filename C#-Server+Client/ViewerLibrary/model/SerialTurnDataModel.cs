using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ViewerLibrary.model
{
    public class SerialTurnDataModel : ITurnDataModel
    {
        private object LOCK = new object();
        private readonly Queue<Turn> queue = new Queue<Turn>();

        private readonly Semaphore semaphore = new Semaphore(0, int.MaxValue);

        private bool streamEnded = false;

        /// <summary>
        /// Add next turn to queue.
        /// </summary>
        /// <param name="turn">added turn</param>
        /// <param name="last">is this turn last one.</param>
        public void Add(Turn turn, bool last)
        {
            lock (LOCK)
            {
                queue.Enqueue(turn);
                streamEnded = last;
                semaphore.Release();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// It block thread when no turn is present.
        /// </summary>
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

        /// <inheritdoc />
        /// <summary>
        /// Implementation need to call Next only after calling HasNext.
        /// </summary>
        public Turn Next()
        {
            lock (LOCK)
            {
                try {
                    return queue.Dequeue();
                } catch (InvalidOperationException) {
                    return null;
                }
            }
        }
    }
}
