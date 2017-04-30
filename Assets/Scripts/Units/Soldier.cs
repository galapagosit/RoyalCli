using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : UnitBase {
	// Use this for initialization
	public override void Start () {
		base.Start ();
		SetHp ();
		InitHpDisplay ();
	}

	void SetHp () {
		hp_current = 1000;
		hp_max = 1000;
	}

	// Update is called once per frame
	void Update () {
		transform.LookAt (targetCastle.transform);

		float dis = Vector3.Distance(transform.position, targetCastle.transform.position);
		if (dis < 100) {
			animator.SetBool ("attack", true);
		} else {
			transform.position += transform.forward * 1 * 2;
			animator.SetBool ("attack", false);
		}
	}
}
