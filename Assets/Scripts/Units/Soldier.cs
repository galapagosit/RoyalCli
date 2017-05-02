using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Soldier : UnitBase {

	int ATTACK = 300;

	void Start () {
		animator = transform.FindChild ("skeleton_animated").gameObject.GetComponent<Animator>();
		SetHp ();
		InitHpDisplay ();
	}

	void SetHp () {
		hp_current = 1000;
		hp_max = 1000;
	}

	// Update is called once per frame
	void Update () {
		DetectTarget ();
		MoveAndAttack ();
	}

	void DetectTarget() {
		GameObject[] target_units;
		if (tag == "unit_p1") {
			target_units = GameObject.FindGameObjectsWithTag ("unit_p2");
		} else {
			target_units = GameObject.FindGameObjectsWithTag ("unit_p1");
		}

		Transform[] orderdTransforms = target_units.Select(go => go.transform)
			.OrderBy(t => Vector3.Distance(t.position , transform.position))
			.ToArray();

		if (orderdTransforms.Length > 0) {
			float first_unit_dis = Vector3.Distance (transform.position, orderdTransforms[0].position);
			float castle_dis = Vector3.Distance (transform.position, targetCastle.transform.position);
			if (first_unit_dis < castle_dis) {
				target = orderdTransforms[0].gameObject;
			} else {
				target = targetCastle;
			}
		} else {
			target = targetCastle;
		}
	}

	void MoveAndAttack () {
		transform.LookAt (target.transform);

		float dis = Vector3.Distance(transform.position, target.transform.position);
		if (dis < 100) {
			animator.SetBool ("attack", true);
		} else {
			transform.position += transform.forward * 1 * 2;
			animator.SetBool ("attack", false);
		}
	}

	public void AttackHit () {
		target.SendMessage("AddDamage", ATTACK);
	}
}
