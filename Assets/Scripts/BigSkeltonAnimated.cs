using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSkeltonAnimated : MonoBehaviour {
	void Hit () {
		transform.parent.GetComponent<BigSoldier> ().AttackHit ();
	}
}
