using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationTrigger : MonoBehaviour {
	void Hit () {
		transform.parent.SendMessage("AttackHit");
	}
}
