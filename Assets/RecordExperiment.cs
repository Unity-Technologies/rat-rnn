using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RecordExperiment : MonoBehaviour
{
	public string filename = "experiment";
	public int length = 50000;
	public int flushToFileEvery = 10000;
	public Controller agent;

	public int screenResolution = 64;

	private int fixedUpdateCounter = 0;
	private List<Controller.ObservableState> entries = new List<Controller.ObservableState>();

	void Start()
	{
		if (agent == null)
			agent = GameObject.FindObjectOfType<Controller>();

		entries = new List<Controller.ObservableState>(flushToFileEvery);

		Screen.SetResolution(screenResolution, screenResolution, false);
	}

	void FixedUpdate()
	{
		if (fixedUpdateCounter++ > length)
			Application.Quit();

		if (agent == null)
			return;
		
		entries.Add(agent.observableState);

		if (entries.Count > flushToFileEvery)
		{
			Save(agent, entries.ToArray());
			entries.Clear();
		}
	}

	void Save(Controller src, Controller.ObservableState[] entries)
	{
		var experiment = new Experiment{
			runSpeed = src.runSpeed,
			turnSpeed = src.turnSpeed,
			bodyRadius = src.gameObject.GetComponent<Collider>().bounds.extents.z,
			sceneName = SceneManager.GetActiveScene().name,
			environmentName = GameObject.FindGameObjectWithTag("Maze").name,
			dateTime = DateTime.UtcNow.ToString(),
			entries = entries
		};
		var json = JsonUtility.ToJson(experiment);

		using (var fs = new FileStream(filename + ".json", FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
	}

	void OnApplicationQuit()
	{
		if (agent == null)
			return;
		
		Save(agent, entries.ToArray());
	}

	class Experiment
	{
		public float runSpeed;
		public float turnSpeed;
		public float bodyRadius;
		public string sceneName;
		public string environmentName;
		public string dateTime;
		public Controller.ObservableState[] entries;
	}
}
