using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {
    private Transform ourBug;

    void Awake()
    {
        ourBug = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private int verticalAxis = 0;
    private Vector3 velocityCameraFollow;
    public Vector3 behindPosition = new Vector3(0,2,-4);
    public float angle;
    public float cameraPullSpeed;
    private void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.W) && ourBug.GetComponent<BugMovementScript>().stamina > 0)
        //{
        //    verticalAxis = 1;

        //}
        //else if (Input.GetKey(KeyCode.S) && ourBug.GetComponent<BugMovementScript>().stamina > 0)
        //{
        //    verticalAxis = -1;
        //}
        //else
        //{
        //    verticalAxis = 0;
        //}
        transform.position = Vector3.SmoothDamp(transform.position, ourBug.transform.TransformPoint(behindPosition) + Vector3.up * verticalAxis, ref velocityCameraFollow, cameraPullSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(angle, ourBug.GetComponent<BugMovementScript>().currentYRotation, 0));
    }
}
