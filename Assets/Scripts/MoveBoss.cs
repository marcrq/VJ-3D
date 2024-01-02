using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoss : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = GameObject.Find("Wacho").transform;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            transform.LookAt(player);
        }
    }
}

