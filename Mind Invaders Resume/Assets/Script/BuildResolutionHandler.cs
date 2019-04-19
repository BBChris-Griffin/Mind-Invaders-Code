using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BuildResolutionHandler : MonoBehaviour {

	private Resolution game;
	public float pixelsPerUnit = 32f;

	// Use this for initialization
	void Start ()
	{
		game = Screen.resolutions[0];
		CalculateSize();
	}

	public void CalculateSize()
	{
		// Landscape = width/height. Portrait = height/width.
		Camera.main.aspect = game.width / game.height;
		Camera.main.orthographicSize = game.width / ((( game.width / game.height ) * 1f) * pixelsPerUnit);
	}
}
