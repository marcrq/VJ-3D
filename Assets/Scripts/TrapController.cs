using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private bool isIn;
    private float temp;
    LivesPlayer livesPlayer;

    void Start()
    {
        temp = Time.time;
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            livesPlayer = player.GetComponent<LivesPlayer>();
        }
    }

    void Update()
    {
        if (isIn && Time.time - temp > 1f)
        {
            livesPlayer.LoseLife();
            temp = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = false;
        }
    }
}
