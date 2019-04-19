using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPauser : MonoBehaviour {

	public AudioSource audio;
    public float pausedVolume;
    public float regVolume;

	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 0)
        {
			//audio.Pause ();
			audio.volume = pausedVolume;
		} else
        {
			//audio.UnPause ();
			audio.volume = regVolume;
		}
	}
}
