using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    //Variables of the moving platform

    //Varibles for playing the fail sound once the player hit
    public AudioClip fail;
    AudioSource audioSource;

    //AudioSource object of the fail sound
    private AudioSource bgm;
    private bool isPlayed;

    //Boolean for enabling the slow-motion effect
    private bool slowmo;

    //Access the movement script
    private Transform ourBug;
    void Awake()
    {
        ourBug = GameObject.FindGameObjectWithTag("Player").transform;
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

    void Update()
    {
        if (slowmo)
            Time.timeScale = 0.5f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ourBug.GetComponent<BugMovementScript>().stamina = 0;
            bgm.Stop();
            if (!isPlayed) {
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
}