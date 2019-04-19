using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
};

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate = 0.5f;
    private float nextFire = 0.0f;
    private bool timeSlowed;

    public new AudioSource audio;

    private void Start()
    {
        timeSlowed = false;
    }

    private void Update()
    {
        if(Input.GetButton("Fire1") && Time.time > nextFire)
        {
            if(Time.timeScale != 0)
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation); //Just create copy of object at that position
                audio.Play();
            }
        }

        if(GlobalVariables.slowDownTime && !timeSlowed)
        {
            speed *= 2;
            timeSlowed = true;
        }
        else if(!GlobalVariables.slowDownTime && timeSlowed)
        {
            speed /= 2;
            timeSlowed = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        //Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Rigidbody shield;

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement*speed;
        shield = GetComponent<Transform>().GetChild(2).GetComponent<Rigidbody>();
        shield.velocity = GetComponent<Rigidbody>().velocity;
        //Restrict Movement Within Space
        GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

        shield.position = new Vector3
            (
                Mathf.Clamp(shield.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(shield.position.z, boundary.zMin, boundary.zMax)
            );

        //Tilt Ship During Movement
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, -tilt*GetComponent<Rigidbody>().velocity.x);
    }
}
