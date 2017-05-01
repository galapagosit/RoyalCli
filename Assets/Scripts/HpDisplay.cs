using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpDisplay : MonoBehaviour {
	
	void LateUpdate()
	{
		// 親の回転に合わせないで固定する
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	public void UpdateHp(int hp_current, int hp_max)
	{
		string s = hp_current.ToString();
		transform.FindChild ("Text").GetComponent<TextMesh>().text = s;

		var tt = transform.FindChild ("Bar").transform;
		var l_scale = tt.localScale;
		l_scale.x = 100 * (hp_current / hp_max);
		tt.localScale = l_scale;
	}
}