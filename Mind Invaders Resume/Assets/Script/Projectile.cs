using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    public float wait;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
       
        if ((tag == "AnswerChoice" || tag == "CorrectAnswer") && wait != 0)
        {
            StartCoroutine(MoveToAttention());
        }
	}

    private void Update()
    {
        if(GlobalVariables.slowDownTime && tag == "Projectile")
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed * 2;
        }
        else if(!GlobalVariables.slowDownTime && tag == "Projectile")
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }

    IEnumerator MoveToAttention()
    {
        if (GlobalVariables.slowDownTime)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed * 2;
            yield return new WaitForSeconds(wait/2);
        }
        else
        {
            yield return new WaitForSeconds(wait);
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
