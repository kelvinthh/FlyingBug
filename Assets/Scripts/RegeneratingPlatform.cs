using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegeneratingPlatform : MonoBehaviour {
    private Transform ourBug;
    private bool onPlatform;


    // Use this for initialization
    void Awake () {
        ourBug = GameObject.FindGameObjectWithTag("Player").transform;
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onPlatform = false;
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
