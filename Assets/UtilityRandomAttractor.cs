using UnityEngine;
using System.Linq;

public class UtilityRandomAttractor : UtilityBase
{
	public float spawnAttractorWithin = 5f;

	private Vector3 attractor;
	private float bodyRadius;
	private float lastSensorSum = 0.0f;

	void Start()
	{
		attractor = transform.position;
		bodyRadius = GetComponent<Collider>().bounds.extents.z;
	}

	protected override Vector2 DoControl(float[] sensors)
	{
		bool requestNewAttractor = false;
		var toAttractor = attractor - transform.position;
		if (toAttractor.magnitude < bodyRadius) // if reached attractor
			requestNewAttractor = true;

		// ALTERNATIVE: maybe better solution would be to check, if attractor is in the direction of the specific blocked sensor
		// but it requires to remember direction of the sensors

		if (sensors.Sum() > lastSensorSum) // if things are getting worse
			if (!requestNewAttractor && Vector3.Dot(Vector3.Normalize(toAttractor), transform.right) <= 0.1f) // and attractor is in front of us
					requestNewAttractor = true; // then attractor is most likely behind the wall, regenerate
		lastSensorSum = sensors.Sum();

		if (requestNewAttractor)
		{
			do {
				var pos2d = Random.insideUnitCircle * Mathf.Max(spawnAttractorWithin, bodyRadius);
				toAttractor = new Vector3(pos2d.x, 0, pos2d.y);
			} while (toAttractor.magnitude < Mathf.Epsilon);
			attractor = transform.position + toAttractor;
		}

		var sharpenTurns = 3.0f;
		var inputX = Mathf.Clamp(Vector3.Dot(Vector3.Normalize(toAttractor), transform.right) * sharpenTurns, -1f, 1f);
		var inputY = Mathf.Clamp01(Mathf.Sqrt(Mathf.Max(0.0f, toAttractor.magnitude - bodyRadius * 0.5f))); // slow down when close to attractor

		return new Vector2(
			inputX,		// angular velocity
			inputY);	// forward velocity
	}

	void Update()
	{
		Debug.DrawLine(transform.position, attractor, Color.yellow, 0, false);
	}
}
