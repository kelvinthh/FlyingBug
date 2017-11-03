using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovementScript : MonoBehaviour {

    Rigidbody ourBug;
	
	//Variables for Up/Down Movement
    public float upForce;
	
	//Varibles for Forward/Backward Movment
    private float movementForwardSpeed = 500.0f;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward; //Unecessary
	
	//Variables for Rotation
	private float wantedYRotation;
    public float currentYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotateYVelocity;
	
	//Variables for Limiting Speed
	[SerializeField]
    private float bugMaxSpeed;
	
	//Variables for Stamina
	[SerializeField]
	public float stamina = 5;
	[SerializeField]
	private float maxStamina = 5;

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
        ourBug.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways));

        if (stamina <= 0)
        {
            upForce = -98.1f;
            stamina = 0;
        }
        if(stamina<maxStamina && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            stamina += Time.deltaTime;
        }
    }

    void MovementUpDown()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (stamina > 0)
            {
                upForce = 450.0f;
                stamina -= Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            upForce = -200.0f;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            //It was positive in the tutorial but I want it to fall when no button is being pressed
            //upForce = -98.1f;
            upForce = -98.1f;
        }
        
    }

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

    void LimitSpeed()
    {
        if (stamina > 0)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                ourBug.velocity = Vector3.ClampMagnitude(ourBug.velocity, bugMaxSpeed);
            }
        }
        else if(stamina <= 0)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                    ourBug.velocity = new Vector3(0, ourBug.velocity.y, 0);

            }
        }
    }

    private float sideMovementAmount = 300.0f;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;
    void Swerve()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if(stamina > 0)
            {
                ourBug.AddRelativeForce(Vector3.right * -sideMovementAmount);
                tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 20, ref tiltAmountVelocity, 0.1f);
            }
            stamina -= Time.deltaTime;
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
        }

        if (Input.GetKey(KeyCode.D))
        {

            if(stamina > 0)
            {
                ourBug.AddRelativeForce(Vector3.right * sideMovementAmount);
                tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -20, ref tiltAmountVelocity, 0.1f);
            }
            stamina -= Time.deltaTime;
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
        }
    }
}
