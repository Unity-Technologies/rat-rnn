using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAgent : Agent
{
    public int episode;

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        state.Add(transform.position.x);
        state.Add(transform.position.z);
        state.Add(transform.rotation.y);
        state.Add(GetComponent<Rigidbody>().velocity.x);
        state.Add(GetComponent<Rigidbody>().velocity.z);
        state.Add(episode);
        return state;
    }

    public override void AgentStep(float[] act)
    {

    }

    public override void AgentReset()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f));
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        episode += 1;
    }

    public override void AgentOnDone()
    {

    }
}
