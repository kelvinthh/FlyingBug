using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovingObstacle : MonoBehaviour
{
    public Transform destinationObejct;
    private Vector3 startPos;
    private Vector3 endPos;
    public float movingTime = 5f;

    void Start()
    {
        startPos = transform.position;
        endPos = destinationObejct.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(startPos, endPos,
         Mathf.SmoothStep(0f, 1f,Mathf.PingPong(Time.time / movingTime, 1f)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}