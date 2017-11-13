using UnityEngine;
using System.Collections;

public class UtilityExternalControl : UtilityBase {

	void Start() {}

	protected override Vector2 DoControl(float[] sensors)
	{
		return new Vector2(
			Input.GetAxis("Horizontal"),	// angular velocity
			Input.GetAxis("Vertical"));	// forward velocity
	}
}
