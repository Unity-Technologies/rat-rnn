using UnityEngine;
using System.Collections;

public class UtilityRandomWalk : UtilityBase {

	public float runSpeed = .1f;
	public float turnSpeed = 30f;

	protected override Vector2 DoControl(float[] sensors)
	{
		var inputX = Random.Range(-1f, 1f);
		var inputY = Random.Range(0, 1f);

		return new Vector2(
			inputX * turnSpeed,	// angular velocity
			inputY * runSpeed);	// forward velocity
	}
}
