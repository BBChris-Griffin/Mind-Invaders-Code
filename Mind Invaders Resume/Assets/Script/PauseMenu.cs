using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject mainPanel;
    private bool active = false;
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 0 && !active)
        {
            mainPanel.SetActive(true);
            active = true;
        }
        else if (Time.timeScale != 0)
        {
            mainPanel.SetActive(false);
            active = false;
        }
	}
}
