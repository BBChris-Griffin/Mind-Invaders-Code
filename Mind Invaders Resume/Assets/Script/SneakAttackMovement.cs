using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakAttackMovement : MonoBehaviour {

    public float speed;
    public float smoothing;

    private Vector3 relativePos;
    private Transform player;
    private Rigidbody rb;
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (player != null)
        {
            relativePos = rb.transform.position - player.transform.position;
            float xMovement = Mathf.MoveTowards(rb.transform.position.x, player.transform.position.x, Time.deltaTime * smoothing);
            float zMovement = Mathf.MoveTowards(rb.transform.position.z, player.transform.position.z, Time.deltaTime * smoothing);

            rb.velocity = new Vector3(xMovement * speed, 0.0f, zMovement * speed);
            rb.rotation = Quaternion.LookRotation(-relativePos);
        }
       
    }

}
