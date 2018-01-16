using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BugMovementScript : MonoBehaviour
{

    Rigidbody ourBug;

    //Variables for Up/Down Movement
    public float upForce;

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
    public float stamina = 8;
    [SerializeField]
    public float maxStamina = 8;

    //Variables for Stamina Bar
    Rect staminaRect;
    public Texture2D staminaTexture;

    //Varibles for playing the fail sound once the player hit
    public AudioClip fail;
    AudioSource audioSource;

    //AudioSource object of the fail sound
    private AudioSource bgm;
    private bool isPlayed;

    //Boolean for enabling the slow-motion effect
    private bool slowmo;

    public GameObject FullGUI;
    public GameObject Warning;

    void Awake()
    {
        ourBug = GetComponent<Rigidbody>();

        //Display the Stamina Bar
        staminaRect = new Rect(Screen.width / 10, Screen.height * 9 / 10, Screen.width / 3, Screen.height / 50);
        staminaTexture = new Texture2D(1, 1);
        staminaTexture.SetPixel(0, 0, Color.white);
        staminaTexture.Apply();

        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isPlayed = false;
        slowmo = false;

        //Reset the slow-motion effect
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    void OnGUI()
    {
        float ratio = stamina / maxStamina;
        float rectWidth = ratio * Screen.width / 3;
        staminaRect.width = rectWidth;
        GUI.DrawTexture(staminaRect, staminaTexture);

    }

    void FixedUpdate()
    {
        MovementUpDown();
        MovementForward();
        Rotation();
        LimitSpeed();
        Swerve();
        Fail();
        ShowFull();
        ourBug.AddRelativeForce(Vector3.up * upForce);
        ourBug.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways));

        if (stamina <= 0)
        {
            upForce = -1000f;
            stamina = 0;
        }
        //if (stamina < maxStamina && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        //{
        //    stamina += Time.deltaTime;
        //}
        if (slowmo)
            Time.timeScale = 0.5f;
    }

    private void ShowFull()
    {
        if(stamina >= maxStamina)
        {
            FullGUI.SetActive(true);
            Warning.SetActive(false);
        }
        if(stamina < maxStamina)
        {
            FullGUI.SetActive(false);
            if(stamina < maxStamina/2)
                Warning.SetActive(true);
        }
    }

    void Fail()
    {
        if(ourBug.position.y  <= -30)
        {
            stamina = 0;
            bgm.Stop();
            if (!isPlayed)
            {
                audioSource.PlayOneShot(fail, 1f);
                isPlayed = true;
            }
            slowmo = true;
            Invoke("Restart", 1);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MovementUpDown()
    {
        if (Input.GetKey(KeyCode.Space))
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

        if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.S))
        {
            //It was positive in the tutorial but I want it to fall when no button is being pressed
            //upForce = -98.1f;
            if(stamina > 0)
                upForce = -250.0f;
        }

    }
    //Varibles for Forward/Backward Movment
    private float movementForwardSpeed = 600.0f;
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
        else if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
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
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                //ourBug.velocity = Vector3.ClampMagnitude(ourBug.velocity, bugMaxSpeed);
            }

            if (ourBug.velocity.z > bugMaxSpeed)
            {
                //ourBug.velocity.z = 10;
                Vector3 tempVel = ourBug.velocity;
                tempVel.z = bugMaxSpeed;
                ourBug.velocity = tempVel;
            }

        }
        else if (stamina <= 0)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                if (ourBug.velocity.z > bugMaxSpeed)
                {
                    //ourBug.velocity.z = 10;
                    Vector3 tempVel = ourBug.velocity;
                    tempVel.z = bugMaxSpeed;
                    ourBug.velocity = tempVel;
                }

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
            if (stamina > 0)
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

            if (stamina > 0)
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