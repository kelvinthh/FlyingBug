using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovementScriptOriginal : MonoBehaviour
{

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
        ClampingSpeedValue();

        ourBug.AddRelativeForce(Vector3.up * upForce);
        ourBug.rotation = Quaternion.Euler(
                new Vector3(tiltAmountForward, currentYRotation, ourBug.rotation.z)
            );
    }

    public float upForce;

    void MovementUpDown()
    {
        if (Input.GetKey(KeyCode.I))
        {
            upForce = 450.0f;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            upForce = -200.0f;
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
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
        if (Input.GetAxis("Vertical") != 0)
        {
            ourBug.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward * Input.GetAxis("Vertical"), 20, ref tiltVelocityForward, 0.1f);
        }
    }

    private float wantedYRotation;
    private float currentYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotateYVelocity;

    void Rotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantedYRotation += rotateAmountByKeys;
        }
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotateYVelocity, 0.25f);
    }

    private Vector3 velocityToSmoothDampToZero;

    void ClampingSpeedValue()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        //if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
        {
            ourBug.velocity = Vector3.ClampMagnitude(ourBug.velocity, Mathf.Lerp(ourBug.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourBug.velocity = Vector3.ClampMagnitude(ourBug.velocity, Mathf.Lerp(ourBug.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourBug.velocity = Vector3.ClampMagnitude(ourBug.velocity, Mathf.Lerp(ourBug.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourBug.velocity = Vector3.SmoothDamp(ourBug.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }

    }
}
