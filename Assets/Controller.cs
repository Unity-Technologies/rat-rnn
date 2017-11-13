using UnityEngine;
using System;
using System.Linq;

public class Controller : MonoBehaviour {
	
	public float runSpeed = .1f;
	public float turnSpeed = 10f;

	public int whiskersCount = 10;
	public float whiskersAngle = 120;
	public float whiskersLength = 1;

	private Rigidbody body;
	private float bodyRadius;

	private ObservableState state;
	public ObservableState observableState { get { return state; } }

	[Serializable]
	public struct ObservableState
	{
		public float heading, velocity;
		public float x, y;
		public float[] sensors;
	}

	void Start()
	{
		body = GetComponent<Rigidbody>();
		bodyRadius = GetComponent<Collider>().bounds.extents.z;
	}

	void FixedUpdate()
	{
		var sensors = ReadSensors();

		var utilities = GetComponents<UtilityBase>();
		var sumPriority = utilities.Sum(u => u.priority * (u.isActiveAndEnabled ? 1: 0f));
		sumPriority = Mathf.Max(sumPriority, Mathf.Epsilon);

		Vector2 control = Vector2.zero;
		foreach (var u in utilities)
			if (u.isActiveAndEnabled)
				control += u.Control (sensors) * u.priority / sumPriority;

		body.angularVelocity = Vector3.up * Mathf.Deg2Rad * Mathf.Clamp(control.x, -1f, 1f) * turnSpeed;
		body.velocity = transform.forward * Mathf.Clamp01(control.y) * runSpeed;

		state = new ObservableState {
			heading = Mathf.Clamp(control.x, -1f, 1f), velocity = Mathf.Clamp01(control.y),		// in
			x = transform.position.x, y = transform.position.y, 								// out
			sensors = sensors																	// aux
		};
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
