using UnityEngine;
using System.Linq;

public class UtilityRandomWalk : UtilityBase {

	public float twitchiness = 0.7f;
	public bool turnAwayFromWalls = true;

	void Start() {}

	protected override Vector2 DoControl(float[] sensors)
	{
		var leftX = twitchiness;
		var rightX = twitchiness;

		// ALTERNATIVE: turnAwayFromWalls could be split into separte Utility
		// by bumping up priority to override other utilities
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

		return new Vector2(
			Random.Range(-leftX, rightX),	// angular velocity
			Random.Range(0, 1f));			// forward velocity
	}
}
