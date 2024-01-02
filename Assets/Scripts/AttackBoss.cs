using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float attackInterval = 2.0f;
    private float lastAttackTime;

    MovePlayer playerScript;
    public GameObject bulletHelper;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Wacho");
        if (player != null)
        {
            playerScript = player.GetComponent<MovePlayer>();
        }
        lastAttackTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.level == 5 && Time.time - lastAttackTime > attackInterval)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
            var bullet = (GameObject)Instantiate(
            bulletPrefab,
            spawnPosition,
            transform.rotation);
            bullet.transform.parent = bulletHelper.transform;
            lastAttackTime = Time.time;
        }
    }
}
