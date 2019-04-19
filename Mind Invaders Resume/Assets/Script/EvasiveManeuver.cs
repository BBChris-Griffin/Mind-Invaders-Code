using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {

    public float dodge;
    public float smoothing;
    public float tilt;
    public float dodgerMoveWait;
    public Boundary boundary;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;

    private float targetManeuver;
    private float currentSpeed;

    private Rigidbody rb;
    private Transform playerTransform;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
	}

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        float evadePosition;
        while (true)
        {
            if (tag == "Dodger")
            {
                if(transform.position.x > 0)
                {
                    targetManeuver = boundary.xMin; //Stays within the boundary
                    evadePosition = transform.position.x;
                }
                else
                {
                    targetManeuver = boundary.xMax; //Stays within the boundary
                    evadePosition = transform.position.x;
                }
                yield return new WaitForSeconds((boundary.xMax + Mathf.Abs(evadePosition)) * dodgerMoveWait);
            }
            else
            {
                // Goes side to side
                targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x); //Stays within the boundary
                yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
                targetManeuver = 0;
                yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
            }
        }
    }

    void FixedUpdate ()
    {
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
        if(tag == "Dodger")
        {
            rb.position = new Vector3
            (
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );
        }  

        //Tilt Ship During Movement
        rb.rotation = Quaternion.Euler(0.0f, 180.0f, -tilt * rb.velocity.x);
    }
}
