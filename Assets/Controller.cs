using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.IO;

public class Controller : MonoBehaviour {
	
	public float runSpeed = .1f;
	public float turnSpeed = 10f;

	public int whiskersCount = 10;
	public float whiskersAngle = 120;
	public float whiskersLength = 1;

	private Rigidbody body;
	private float bodyRadius;

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

		RecordState(
			Mathf.Clamp(control.x, -1f, 1f), Mathf.Clamp01(control.y),	// in
			transform.position.x, transform.position.z,					// out
			sensors														// aux
		);

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

	System.Collections.Generic.List<StateEntry> entries = new System.Collections.Generic.List<StateEntry>(1000);
	void RecordState(float heading, float velocity, float x, float y, float[] sensors)
	{	
		entries.Add(
			new StateEntry{
				heading = heading, velocity = velocity,
				x = x, y = y, 
				sensors = sensors
			});

		if (entries.Count > 100)
			SaveExperiment();
	}

	void SaveExperiment()
	{
		var experiment = new Experiment{
			runSpeed = runSpeed,
			turnSpeed = turnSpeed,
			bodyRadius = bodyRadius,
			sceneName = SceneManager.GetActiveScene().name,
			environmentName = GameObject.FindGameObjectWithTag("Maze").name,
			dateTime = DateTime.UtcNow.ToString(),
			entries = entries.ToArray()
		};
		var json = JsonUtility.ToJson(experiment);

		var filename = "experiment.json";
		using (var fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
		{
			bool newFile = fs.Length == 0;

			if (!newFile)
				fs.Seek(-1, SeekOrigin.End); // remove closing bracket, wwe are going to add entry to the array

			using (var writer = new StreamWriter(fs))
			{
				if (newFile)
					writer.Write("[\n");
				else
					writer.Write(",\n");
				writer.Write(json);
				writer.Write("]");
			}
		}

		entries.Clear();
	}

	void OnApplicationQuit()
	{
		SaveExperiment();
	}

	class Experiment
	{
		public float runSpeed;
		public float turnSpeed;
		public float bodyRadius;
		public string sceneName;
		public string environmentName;
		public string dateTime;
		public StateEntry[] entries;
	}

	[Serializable]
	struct StateEntry
	{
		public float heading, velocity;
		public float x, y;
		public float[] sensors;
	}
}
