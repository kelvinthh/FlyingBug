using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpinningObstacle : MonoBehaviour
{

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

    //Spinning Axis
    [SerializeField]
    private float rotateX;
    [SerializeField]
    private float rotateY;
    [SerializeField]
    private float rotateZ;

    void Awake()
    {
        ourBug = GameObject.FindGameObjectWithTag("Player").transform;
        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
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
        transform.Rotate(new Vector3(rotateX, rotateY, rotateZ) * Time.deltaTime);
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