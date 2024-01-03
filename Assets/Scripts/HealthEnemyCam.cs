using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemyCam : MonoBehaviour
{
    public GameObject enemy;
    void Update() {
        transform.position = enemy.transform.position;
        transform.LookAt(Camera.main.transform);
    }

}
