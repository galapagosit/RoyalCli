using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour {

	public GameObject targetCastle;
	public Animator animator;

	public GameObject HpDisplayPrefab;
	public GameObject hpDisplay;

	public int hp_current;
	public int hp_max;

	public GameObject target;

	// Use this for initialization
	public virtual void Start () {
		animator = transform.FindChild ("skeleton_animated").gameObject.GetComponent<Animator>();
	}

	public virtual void InitHpDisplay () {
		hpDisplay = (GameObject)Instantiate (HpDisplayPrefab);
		hpDisplay.transform.parent = transform;
		hpDisplay.transform.localPosition = new Vector3(0, 0, 0);
	}

	void AddDamage(int damage)
	{
		hp_current -= damage;
		if (hp_current <= 0) {
			Destroy (gameObject);
		}
	}

	void LateUpdate()
	{
		hpDisplay.GetComponent<HpDisplay>().UpdateHp(hp_current, hp_max);
	}
}
