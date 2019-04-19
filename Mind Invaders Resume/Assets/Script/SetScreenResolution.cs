using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreenResolution : MonoBehaviour {

    public int width;
    public int height;
    public bool isFullScreen;
    public int desiredFPS;
    void Start()
    {
        Screen.SetResolution(width, height, isFullScreen, desiredFPS);
    }
}
