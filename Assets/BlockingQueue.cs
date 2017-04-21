using System.Threading;
using System.Collections.Generic;

public class BlockingQueue<T> {

	Queue<T> queue = new Queue<T>();

	public void Enqueue(T obj) {
		lock (queue) {
			queue.Enqueue(obj);
			if (queue.Count == 1) {
				Monitor.PulseAll(queue);
			}
		}
	}

	public T Dequeue() {
		lock (queue) {
			while(queue.Count == 0) {
				Monitor.Wait(queue);
			}
			return queue.Dequeue();
		}
	}

}