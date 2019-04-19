using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unpause : MonoBehaviour {

	void Start ()
    {
		if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
	}
}
