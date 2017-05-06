using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour {
	int MAX = 1000;
	public int mana;

	public GameObject ManaNum;

	// Use this for initialization
	void Start () {
		mana = 600;
		ManaNum = transform.FindChild ("ManaNum").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		mana++;
		if (mana > MAX) {
			mana = MAX;
		}
		ManaNum.GetComponent<TextMesh>().text = mana.ToString();
	}

	public bool UseMana (int used) {
		if (used > mana) {
			return false;
		}

		mana -= used;
		return true;
	}
}
