using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;

[Serializable]
public class Frame {
	public Vector3 point;
}


public class Battle : MonoBehaviour {

	bool LOCAL_DEBUG = true;
	int debug_spawn_cnt = 0;

	int f_count = 0;

	public GameObject castle1;
	public GameObject castle2;

	public GameObject mana1;
	public GameObject mana2;

	public GameObject Soldier;
	public GameObject BigSoldier;

	public Frame currentFrame;

	FrameWorker frameWorker;

	public string UnitSelectButtonName = null;

	void Awake() {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 30; //30FPSに設定
	}

	// Use this for initialization
	void Start () {
		if (!LOCAL_DEBUG) {
			frameWorker = new FrameWorker ();
			ReadyAndWaitOpponent ();
		}
	}

	// Update is called once per frame
	void Update () {
		currentFrame = null;
		HandleTap ();

		if (!LOCAL_DEBUG) {
			SendFrame ();

			if (f_count >= 30) {
				RecvFrame ();
			}
		}
		f_count++;
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
		Ray ray = Camera.main.ScreenPointToRay(position);
		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast (ray, out hit, 2000)) {
			currentFrame = new Frame () {
				point = hit.point,
			};

			if (LOCAL_DEBUG) {
				if (debug_spawn_cnt % 2 == 0) {
					SpawnUnit (castle2, currentFrame.point);
				} else {
					SpawnUnit (castle1, currentFrame.point);
				}
				debug_spawn_cnt++;
			}
		}
	}

	void ReadyAndWaitOpponent () {
		frameWorker.send (f_count + "/ready");

		for(int x = 0; x < 20; ++x){
			string msg = frameWorker.recv ();
			if (msg == null) {
				Debug.Log ("waiting opponent...");
				Thread.Sleep (1000);
			} else {
				Debug.Log ("opponent appear" + msg);
				return;
			}
		}
		Debug.Log ("no opponent");
	}

	void SendFrame () {
		// このプレイヤーのフレームデータを送信
		string message;
		if (currentFrame == null) {
			message = f_count + "/-";
		} else {
			message = f_count + "/" + JsonUtility.ToJson (currentFrame);
		}
		frameWorker.send (message);
	}

	void RecvFrame () {
		// 両方のプレイヤーのフレームデータを処理
		string msg = frameWorker.recv ();
		Debug.Log ("f_count:" + f_count + " msg:" + msg);
		string[] frames = msg.Split('#');

		// プレイヤー1のフレームデータを処理
		string[] frameP1 = frames[0].Split('/');
		if (frameP1 [1] != "-") {
			Frame frame1 = JsonUtility.FromJson<Frame> (frameP1 [1]);
			SpawnUnit (castle2, frame1.point);
		}

		// プレイヤー2のフレームデータを処理
		string[] frameP2 = frames[1].Split('/');
		if (frameP2 [1] != "-") {
			Frame frame2 = JsonUtility.FromJson<Frame> (frameP2 [1]);
			SpawnUnit (castle1, frame2.point);
		}
	}
		
	private void SpawnUnit(GameObject targetCastle, Vector3 point)
	{
		string unit_tag;
		GameObject mana;
		if (targetCastle == castle1) {
			unit_tag = ("unit_p2");
			mana = mana2;
		} else {
			unit_tag = ("unit_p1");
			mana = mana1;
		}

		GameObject Unit = null;
		if (UnitSelectButtonName == "Button1") {
			Unit = Soldier;
		} else if (UnitSelectButtonName == "Button2") {
			Unit = BigSoldier;
		} else {
			Debug.Log ("no unit button selected");
		}

		UnitBase u_base = Unit.GetComponent<UnitBase>();
		bool used = mana.GetComponent<Mana> ().UseMana (u_base.GetCost());
		if (!used) {
			Debug.Log ("mana is not enought");
			return;
		}

		GameObject unit = (GameObject)Instantiate (Unit, point, new Quaternion ());
		UnitBase s = unit.GetComponent<UnitBase>();
		s.targetCastle = targetCastle;
		s.tag = unit_tag;
	}

	private void OnDestroy()
	{
		if (!LOCAL_DEBUG) {
			frameWorker.stop ();
		}
	}

	public void SelectUnit()
	{

	}
}
