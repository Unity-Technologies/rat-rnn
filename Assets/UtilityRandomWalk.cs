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

	/*private Rigidbody rb;
	private float nextUpdate = -1f;
	private float forwardSpeed = 0f;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{			
		if (nextUpdate <= Time.time)
		{
			if (nextUpdate < 0)
				nextUpdate = Time.time;

			int maxIterations = 3;
			while (nextUpdate < Time.time && maxIterations-- > 0)
			{
				nextUpdate += 1.0f / decisionFrequency;

				var inputX = Random.Range(-1f, 1f);
				var inputY = Random.Range(0, 1f);

				rb.angularVelocity = Vector3.up * inputX * Mathf.Deg2Rad * turnSpeed / Time.deltaTime;
				forwardSpeed = inputY * runSpeed / Time.deltaTime;
			}
		}

		rb.velocity = transform.forward * forwardSpeed;

	}*/
}