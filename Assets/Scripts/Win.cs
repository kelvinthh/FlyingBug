using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour {

    public AudioClip win;
    AudioSource audioSource;
    private AudioSource bgm;
    private bool isPlayed;
    public GameObject vicText;

	// Use this for initialization
	void Awake () {
        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isPlayed = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        bgm.Stop();
        if (!isPlayed)
        {
            audioSource.PlayOneShot(win, 1f);
            isPlayed = true;
            vicText.SetActive(true);
        }
        Invoke("Restart", 10);
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
