using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRegen : MonoBehaviour
{

    public Transform destinationObejct;
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField]
    private float movingTime = 5f;

    private Transform ourBug;
    private bool onPlatform;

    public AudioClip healing;
    AudioSource audioSource;
    private bool isPlayed;


    // Use this for initialization
    void Awake()
    {
        ourBug = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        startPos = transform.position;
        endPos = destinationObejct.position;
        audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(startPos, endPos,
        Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.time / movingTime, 1f)));
        if (onPlatform)
        {
            Regenerate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onPlatform = true;
            if (!isPlayed)
            {
                audioSource.PlayOneShot(healing, 0.5f);
                isPlayed = true;
            }
            ourBug.GetComponent<BugMovementScript>().staminaTexture.SetPixel(0, 0, Color.green);
            ourBug.GetComponent<BugMovementScript>().staminaTexture.Apply();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onPlatform = false;
            isPlayed = false;
            ourBug.GetComponent<BugMovementScript>().staminaTexture.SetPixel(0, 0, Color.white);
            ourBug.GetComponent<BugMovementScript>().staminaTexture.Apply();
        }
    }

    void Regenerate()
    {
        ourBug.GetComponent<BugMovementScript>().stamina += Time.deltaTime; //Time.deltaTime;
        if (ourBug.GetComponent<BugMovementScript>().stamina > ourBug.GetComponent<BugMovementScript>().maxStamina)
        {
            ourBug.GetComponent<BugMovementScript>().stamina = ourBug.GetComponent<BugMovementScript>().maxStamina;
        }
    }
}
