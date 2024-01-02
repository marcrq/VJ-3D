using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float attackInterval = 3.0f;
    private float lastAttackTime;
    float attackTime = 0.5f;
    bool hasShot;

    public Animator animador;

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
        hasShot = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.level == 5 && Time.time - lastAttackTime > attackInterval)
        {
            hasShot = false;
            animador.SetTrigger("AttackTrigger");
            lastAttackTime = Time.time;
        }

        if (playerScript.level == 5 && !hasShot && Time.time - lastAttackTime > attackTime) {
            hasShot = true;
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
            var bullet = (GameObject)Instantiate(
            bulletPrefab,
            spawnPosition,
            transform.rotation);
            bullet.transform.parent = bulletHelper.transform;
        }
    }

    void ActivateShield()
    {
    }
}
