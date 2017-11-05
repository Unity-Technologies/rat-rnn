using UnityEngine;
using System.Collections;

public abstract class UtilityBase : MonoBehaviour {

	public float strength = 1f;
	public float decisionFrequency = 10f;
	private float nextUpdate = -1f;
	private Vector2 memory = Vector2.zero;

	public Vector2 Control(float[] sensors)
	{
		Vector2 motors = Vector2.zero;

		if (nextUpdate < 0)
			nextUpdate = Time.time;

		int iterations = 0;
		while (nextUpdate <= Time.time && ++iterations <= 10)
		{
			nextUpdate += 1.0f / decisionFrequency;
			motors += DoControl(sensors);
		}

		// if decisions were made => store in memory
		if (iterations > 0)
			memory = motors / iterations;

		// recall last control from memory
		return memory;
	}

	protected abstract Vector2 DoControl(float[] sensors);
}
