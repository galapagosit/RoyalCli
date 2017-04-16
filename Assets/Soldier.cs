using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {
	public GameObject targetCastle;
	public Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
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
