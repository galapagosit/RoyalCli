using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeltonAnimated : MonoBehaviour {
	void Hit () {
		transform.parent.GetComponent<Soldier> ().AttackHit ();
	}
}
