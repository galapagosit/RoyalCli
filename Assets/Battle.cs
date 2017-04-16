using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Text;
using System.Net.Sockets;


[Serializable]
public class Frame {
	public int f_count;
	public Vector3 point;
	public bool placed;
}

public class Battle : MonoBehaviour {
	string server = "popbits.co.jp";
	int port = 8080;

	TcpClient client;
	BinaryReader reader;
	BinaryWriter writer;

	int f_count = 0;

	public GameObject castle1;
	public GameObject castle2;

	public GameObject Soldier;
	public Frame currentFrame;

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
			currentFrame = new Frame () {
				f_count = f_count,
				point = Vector3.zero,
				placed = false
			};
			HandleTap ();
			CommandAndWaitOponent ();
		}
		f_count++;
	}

	void ReadyAndWaitOponent () {
		byte[] dgram = Encoding.UTF8.GetBytes("ready\n");
		writer.Write(dgram);

		Debug.Log ("waiting...");
		byte[] bs = new byte[1024];
		reader.Read(bs, 0, 1024);
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
		Ray ray = Camera.main.ScreenPointToRay(position);
		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast (ray, out hit, 2000)) {
			Debug.Log ("hit something");

			currentFrame = new Frame () {
				f_count = f_count,
				point = hit.point,
				placed = true
			};

//			GameObject soldier = (GameObject)Instantiate (Soldier, hit.point, new Quaternion ());
//			Soldier s = soldier.GetComponent<Soldier>();
//			s.targetCastle = castle2;
		}
	}

	void CommandAndWaitOponent () {
		// このプレイヤーのフレームデータを送信
		string message = JsonUtility.ToJson (currentFrame);
		Debug.Log (message);
		byte[] dgram = Encoding.UTF8.GetBytes(message);
		writer.Write(dgram);

		// 両方のプレイヤーのフレームデータを処理
		Debug.Log ("waiting...");
		byte[] bs = new byte[1024];
		reader.Read(bs, 0, 1024);
		string recv_message = Encoding.UTF8.GetString(bs);
		Debug.Log ("recv_message:" + recv_message);

		string[] frames = recv_message.Split('#');

		// プレイヤー1のフレームデータを処理
		Frame frame1 = JsonUtility.FromJson<Frame> (frames[0]);
		if (frame1.placed) {
			GameObject soldier = (GameObject)Instantiate (Soldier, frame1.point, new Quaternion ());
			Soldier s = soldier.GetComponent<Soldier>();
			s.targetCastle = castle2;
		}

		// プレイヤー2のフレームデータを処理
		Frame frame2 = JsonUtility.FromJson<Frame> (frames[1]);
		if (frame2.placed) {
			GameObject soldier = (GameObject)Instantiate (Soldier, frame2.point, new Quaternion ());
			Soldier s = soldier.GetComponent<Soldier>();
			s.targetCastle = castle1;
		}
	}
}
