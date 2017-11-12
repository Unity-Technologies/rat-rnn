using UnityEngine;
using System.Collections;

public abstract class UtilityBase : MonoBehaviour {

	public float priority = 1f;
	public float decisionFrequency = 10f;
	private double nextUpdate = 0f;
	private Vector2 memory = Vector2.zero;

	public Vector2 Control(float[] sensors)
	{
		Vector2 motors = Vector2.zero;

		int iterations = 0;
		nextUpdate -= (double)Time.fixedDeltaTime;
		while (nextUpdate < 0.0 && ++iterations <= 10)
		{
			nextUpdate += 1.0 / (double)decisionFrequency;
			motors += DoControl(sensors);
		}
		if (nextUpdate < 0)
			nextUpdate = 0;

		// if decisions were made => store in memory
		if (iterations > 0)
			memory = motors / iterations;

		// recall last control from memory
		return memory;
	}

	protected abstract Vector2 DoControl(float[] sensors);
}
