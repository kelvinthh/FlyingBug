using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegeneratingPlatform : MonoBehaviour {
    private Transform ourBug;
    private bool onPlatform;

    public AudioClip healing;
    AudioSource audioSource;
    private bool isPlayed;


    // Use this for initialization
    void Awake () {
        ourBug = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    // Update is called once per frame
    void Update () {
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
