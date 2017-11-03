using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovementScript : MonoBehaviour {

    // Use this for initialization
    Rigidbody ourBug;

    void Awake()
    {
        ourBug = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        MovementUpDown();
        MovementForward();
        Rotation();
        LimitSpeed();
        Swerve();

        ourBug.AddRelativeForce(Vector3.up * upForce);
        ourBug.rotation = Quaternion.Euler(
                new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways)
            );
    }

    public float upForce;

    void MovementUpDown()
    {
        if (Input.GetKey(KeyCode.W))
        {
            upForce = 450.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            upForce = -200.0f;
        }
        else if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            //It was positive in the tutorial but I want it to fall when no button is being pressed
            //upForce = -98.1f;
            upForce = -98.1f;
        }
    }

    //[SerializeField]
    private float movementForwardSpeed = 500.0f;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward; //Unecessary

    void MovementForward()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ourBug.AddRelativeForce(Vector3.forward * movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20, ref tiltVelocityForward, 0.1f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ourBug.AddRelativeForce(Vector3.forward * -movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, -20, ref tiltVelocityForward, 0.1f);
        }
        else if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 0, ref tiltVelocityForward, 0.1f);
        }
    }

    private float wantedYRotation;
    public float currentYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotateYVelocity;

    void Rotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            wantedYRotation += rotateAmountByKeys;
        }
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotateYVelocity, 0.25f);
    }


    [SerializeField]
    private float bugMaxSpeed;
    void LimitSpeed()
    {
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            ourBug.velocity = Vector3.ClampMagnitude(ourBug.velocity, bugMaxSpeed);

    }

    private float sideMovementAmount = 300.0f;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;
    void Swerve()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ourBug.AddRelativeForce(Vector3.right * -sideMovementAmount);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 20, ref tiltAmountVelocity, 0.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            ourBug.AddRelativeForce(Vector3.right * sideMovementAmount);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -20, ref tiltAmountVelocity, 0.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
        }
    }
}
