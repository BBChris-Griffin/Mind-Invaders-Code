using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampAnswer : MonoBehaviour {

    public Text answer;
    private Vector3 namePos;
    private Vector3 startPos;

    // Use this for initialization
    void Awake ()
    {
        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(GetComponentInParent<Rigidbody>().velocity != Vector3.zero)
        {
            transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);
            namePos = Camera.main.WorldToScreenPoint(this.transform.position);
            answer.transform.position = namePos;
        }
       
    }
}
