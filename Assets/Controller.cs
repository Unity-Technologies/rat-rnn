using UnityEngine;
using System;
using System.Linq;

public class Controller : MonoBehaviour {
	
	public float maxRunSpeed = .1f;
	public float maxTurnSpeed = 10f;

	public int whiskersCount = 10;
	public float whiskersAngle = 120;
	public float whiskersLength = 1;

	private Rigidbody rb;
	private float bodyRadius;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		bodyRadius = GetComponent<Collider>().bounds.extents.z;
	}

	void FixedUpdate()
	{
		var sensors = ReadSensors();

		var utilities = GetComponents<UtilityBase>();
		var totalStrength = utilities.Sum(u => u.strength);
		totalStrength = Mathf.Max(totalStrength, Mathf.Epsilon);

		Vector2 control = Vector2.zero;
		foreach (var u in utilities)
			control += u.Control (sensors) * u.strength / totalStrength;

		rb.angularVelocity = Vector3.up * Mathf.Deg2Rad * Mathf.Clamp(control.x, -maxTurnSpeed, maxTurnSpeed) / Time.deltaTime;
		rb.velocity = transform.forward * Mathf.Clamp(control.y, 0, maxRunSpeed) / Time.deltaTime;
	}

	float[] ReadSensors()
	{
		var sensors = new float[whiskersCount];

		var angle = -whiskersAngle / 2.0f;
		for (int q = 0; q < whiskersCount; ++q, angle += whiskersAngle / whiskersCount)
		{
			var dir = Quaternion.Euler(0, angle, 0) * transform.forward;
			var orig = transform.position + dir * bodyRadius;

			RaycastHit hit;
			if (Physics.Raycast(orig, dir, out hit, whiskersLength))
				sensors[q] = 1.0f - hit.distance / whiskersLength;

			Debug.DrawLine(orig, orig + dir * whiskersLength, sensors[q] > 0 ? Color.red: Color.green, 0, false);
		}

		return sensors;
	}
}
