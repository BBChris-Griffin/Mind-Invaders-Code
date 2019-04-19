using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    public GameObject dummyShip;
    public GameObject background;

	void Update ()
    {
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

}
