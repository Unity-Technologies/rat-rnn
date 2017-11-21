using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAcademy : Academy {

	public override void AcademyReset()
	{
		float val;
		int episodeLength = resetParameters.TryGetValue("ep_length", out val) ? (int)val - 1: 299;
		float startAreaExtents = resetParameters.TryGetValue("start_area_extents", out val) ? val : 0F;

		foreach (var c in GameObject.FindObjectsOfType<RatAgent>())
		{
			c.maxStep = episodeLength;
			c.startAreaExtents = new Vector2(startAreaExtents, startAreaExtents);
		}
	}

	public override void AcademyStep()
	{


	}

}
