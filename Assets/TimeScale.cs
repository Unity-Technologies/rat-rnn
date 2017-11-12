using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour {

	public float timeScale = 100.0f;
	public float moreFixedTimeIterations = 1.0f;

	// Use this for initialization
	void Start () {
		Time.timeScale = timeScale;
		Time.fixedDeltaTime /= moreFixedTimeIterations;

		// since we have more iterations, we can increase speed!
		foreach (var c in GameObject.FindObjectsOfType<Controller>())
		{
			c.runSpeed *= moreFixedTimeIterations;
			c.turnSpeed *= moreFixedTimeIterations;
		}

		foreach (var u in GameObject.FindObjectsOfType<UtilityBase>())
		{
			u.decisionFrequency *= moreFixedTimeIterations;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
