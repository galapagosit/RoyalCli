using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets;
using System.Text;
using System.IO;

public class Battle : MonoBehaviour {
	string server = "popbits.co.jp";
	int port = 8080;

	TcpClient client;
	BinaryReader reader;
	BinaryWriter writer;

	int f_count = 0;

	// Use this for initialization
	void Start () {
		InitSock ();
	}

	void InitSock () {
		client = new TcpClient(server, port);
		Stream stream = client.GetStream();
		reader = new BinaryReader(stream);
		writer = new BinaryWriter(stream);
	}

	// Update is called once per frame
	void Update () {
		// 0フレーム目は相手と同期を取ってカウンタを合わせる
		if (f_count == 0) {
			ReadyAndWaitOponent ();
		} else {
			HandleTap ();
			CommandAndWaitOponent ();
		}
		f_count++;
	}

	void ReadyAndWaitOponent () {
		byte[] dgram = Encoding.UTF8.GetBytes("ready\n");
		writer.Write(dgram);

		Debug.Log ("waiting...");
		byte[] bs = new byte[256];
		reader.Read(bs, 0, 256);
		string message = Encoding.UTF8.GetString(bs);
		Debug.Log ("message:" + message);
	}

	void HandleTap ()
	{
		if (Input.touchSupported) {
			foreach (Touch t in Input.touches) {
				switch (t.phase) {
				case TouchPhase.Began:
					CheckPosition (t.position);
					break;
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				CheckPosition (Input.mousePosition);
			}
		}

	}

	void CheckPosition (Vector3 position)
	{
		Debug.Log ("tapped");

		byte[] dgram = Encoding.UTF8.GetBytes("hello!\n");
		writer.Write(dgram);

		Ray ray = Camera.main.ScreenPointToRay(position);
		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast(ray, out hit)){
			Debug.Log ("hit something");
		}
	}

	void CommandAndWaitOponent () {
		string command = "f_count:" + f_count;
		Debug.Log ("command:" + command);
		byte[] dgram = Encoding.UTF8.GetBytes(command);
		writer.Write(dgram);

		Debug.Log ("waiting...");
		byte[] bs = new byte[256];
		reader.Read(bs, 0, 256);
		string message = Encoding.UTF8.GetString(bs);
		Debug.Log ("message:" + message);
	}
}
