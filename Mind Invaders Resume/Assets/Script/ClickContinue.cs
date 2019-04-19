using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickContinue : MonoBehaviour {

    private Pauser pauser;
    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            pauser = gameControllerObject.GetComponent<Pauser>();
        }
        else
        {
            Debug.Log("Cannot Find 'GameController' script");
        }
    }
    public void Continue()
	{
        pauser.Paused();
    }
}
