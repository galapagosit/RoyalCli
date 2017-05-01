using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour {

	public int hp_current;
	public int hp_max;

	public GameObject hpDisplay;

	void Start () {
		SetHp ();
		hpDisplay = transform.FindChild ("HpDisplay").gameObject;
	}

	void SetHp () {
		hp_current = 30000;
		hp_max = 30000;
	}
	
	public void AddDamage(int damage)
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
