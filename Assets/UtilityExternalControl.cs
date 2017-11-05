using UnityEngine;
using System.Collections;

public class UtilityExternalControl : UtilityBase {
	
	public float runSpeed = .1f;
	public float turnSpeed = 30f;

	protected override Vector2 DoControl(float[] sensors)
	{
		var inputX = Input.GetAxis("Horizontal");
		var inputY = Input.GetAxis("Vertical");

		return new Vector2(
			inputX * turnSpeed,	// angular velocity
			inputY * runSpeed);	// forward velocity
	}
}
