using UnityEngine;
using System;
using System.Linq;

public class Controller : MonoBehaviour {
	
	public float maxRunSpeed = .1f;
	public float maxTurnSpeed = 10f;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		var sensors = new float[0];

		// @TODO: whiskers

		var utilities = GetComponents<UtilityBase>();
		var totalStrength = utilities.Sum(u => u.strength);
		totalStrength = Mathf.Max(totalStrength, Mathf.Epsilon);

		Vector2 control = Vector2.zero;
		foreach (var u in utilities)
			control += u.Control (sensors) * u.strength / totalStrength;

		rb.angularVelocity = Vector3.up * Mathf.Deg2Rad * Mathf.Clamp(control.x, -maxTurnSpeed, maxTurnSpeed) / Time.deltaTime;
		rb.velocity = transform.forward * Mathf.Clamp(control.y, 0, maxRunSpeed) / Time.deltaTime;
	}
}
