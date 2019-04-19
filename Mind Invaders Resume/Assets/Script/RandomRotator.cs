using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    public float tumble;

    void Start()
    {
        if(tag == "ExtraLife" || tag == "SlowTimeIcon")
        {
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, tumble);
        }
        else
        {
            GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble; //Creates a rotation for the asteroid
        }
    }
}
