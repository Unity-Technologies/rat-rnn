using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAgent : Agent
{
	public Vector2 startAreaExtents = new Vector2(2.5f, 2.5f);

    public int episode;

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();

		// Y
        state.Add(transform.position.x); // 0
        state.Add(transform.position.z); // 1

		// X
		state.Add(transform.rotation.eulerAngles.y/180.0f-1.0f); // 2
        state.Add(GetComponent<Rigidbody>().velocity.x); // 3
        state.Add(GetComponent<Rigidbody>().velocity.z); // 4
		state.Add(GetComponent<Rigidbody>().angularVelocity.y); // 5
		state.Add(GetComponent<Rigidbody>().velocity.magnitude); // 6

		// intent
		state.Add(GetComponent<Controller>().observableState.heading); // 7
		state.Add(GetComponent<Controller>().observableState.velocity); // 8

		// aux
        state.Add(episode); // 9
        return state;
    }

    public override void AgentStep(float[] act)
    {

    }

    public override void AgentReset()
    {
		transform.position = new Vector3(Random.Range(-startAreaExtents.x, startAreaExtents.x), 0f, Random.Range(-startAreaExtents.y, startAreaExtents.y));
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f));
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        episode += 1;
    }

    public override void AgentOnDone()
    {

    }
}
