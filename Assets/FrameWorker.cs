using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// https://www.codeproject.com/Articles/10649/An-Introduction-to-Socket-Programming-in-NET-using

public class FrameWorker {
	string server = "popbits.co.jp";
	int port = 8080;

	bool shutdown = false;

	TcpClient client;

	StreamReader reader;
	StreamWriter writer;

	Queue<string> send_queue;
	Queue<string> recv_queue;

	Thread send_thread;
	Thread recv_thread;

	public FrameWorker () {
		initSock ();

		send_queue = new Queue<string>();
		recv_queue = new Queue<string>();

		send_thread = new Thread(sender);
		send_thread.Start();

		recv_thread = new Thread(receiver);
		recv_thread.Start();
	}

	void initSock () {
		client = new TcpClient(server, port);
		Stream stream = client.GetStream();
		reader = new StreamReader(stream);
		writer = new StreamWriter(stream);
		writer.AutoFlush = true;
	}

	public void send (string s) {
		lock (send_queue) {
			send_queue.Enqueue (s);
			Monitor.PulseAll (send_queue);
		}
	}

	public string recv () {
		if (recv_queue.Count > 0) {
			return recv_queue.Dequeue ();
		} else {
			return null;
		}
	}

	void sender () {
		while (!shutdown) {
			lock (send_queue) {
				Monitor.Wait (send_queue);
				if (send_queue.Count > 0) {
					string s = send_queue.Dequeue ();
					Debug.Log ("send:" + s);
					writer.WriteLine(s);
				} else {
					Debug.Log ("send_queue is empty...");
				}
			}
		}
	}

	void receiver () {
		while (!shutdown) {
			string recv_message = reader.ReadLine ();
			Debug.Log ("recv:" + recv_message);
			recv_queue.Enqueue (recv_message);
		}
	}

	public void stop(){
		if (send_thread != null)
		{
			send_thread.Abort();
			send_thread = null;
		}
		if (recv_thread != null)
		{
			recv_thread.Abort();
			recv_thread = null;
		}
	}
}
