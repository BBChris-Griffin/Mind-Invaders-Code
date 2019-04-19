using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour {

	protected bool paused = false;

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp("Pause"))
		{
			paused = !paused;
		}

		if (Time.timeScale > 1)
        {
			paused = false;
		}

		if(paused)
        {
            Time.timeScale = 0; //TimeScale determines how fast time moves (multiplication). Now, time is gone.

        }
        else if(!paused && GlobalVariables.slowDownTime)
        {
            Time.timeScale = 0.5f;
        }
        else if(!paused && !GlobalVariables.slowDownTime)
        {
            Time.timeScale = 1; // Time is back to normal
        }
    }

    public void Paused()
    {
        paused = !paused;
    }
}
