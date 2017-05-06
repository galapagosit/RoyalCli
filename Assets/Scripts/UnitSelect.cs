using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour {

	public GameObject battleManager;

	void Start () {
		battleManager = GameObject.Find ("BattleManager");
	}

	public void Selected () {
		var btns = GameObject.FindGameObjectsWithTag ("unit_select_button");
		foreach (var btn in btns)
		{
			var rt = btn.GetComponent<RectTransform> ();
			var tmp_pos = rt.localPosition;
			if (btn == gameObject) {
				tmp_pos.y = -810f;
			} else {
				tmp_pos.y = -830f;
			}
			rt.localPosition = tmp_pos;
		}

		battleManager.GetComponent<Battle> ().UnitSelectButtonName = gameObject.name;
	}
}
