using UnityEngine;
using System.Linq;

public class UtilityRandomWalk : UtilityBase {

	public float runSpeed = .1f;
	public float turnSpeed = 5f;

	public bool turnAwayFromWalls = true;

	protected override Vector2 DoControl(float[] sensors)
	{
		var leftX = 1f;
		var rightX = 1f;

		if (turnAwayFromWalls)
		{
			var n = sensors.Length / 2;
			float l = sensors.Take(n).Reverse().Count(s => s < 0.1f);
			float r = sensors.Skip(n).Count(s => s < 0.1f);

			leftX = Mathf.Lerp(-1f, 1f, l / n);
			rightX = Mathf.Lerp(-1f, 1f, r / n);

			if (-leftX > rightX)
			{
				leftX = -1f;
				rightX = 1f;
			}
		}

		var inputX = Random.Range(-leftX, rightX);
		var inputY = Random.Range(0, 1f);

		return new Vector2(
			inputX * turnSpeed,	// angular velocity
			inputY * runSpeed);	// forward velocity
	}
}
