using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject rangeAttackPrefab;
    public GameObject boomPrefab;

    public float attackInterval = 10.0f;
    private float lastAttackTime;
    float attackTime = 0.5f;
    bool hasShot;
    int attackCount;
    int radius = 5;

    public Animator animador;

    GameObject player;
    MovePlayer playerScript;
    LivesPlayer livesPlayerScript;
    public GameObject bulletHelper;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Wacho");
        if (player != null)
        {
            playerScript = player.GetComponent<MovePlayer>();
            livesPlayerScript = player.GetComponent<LivesPlayer>();
        }
        lastAttackTime = Time.time;
        hasShot = false;

        attackCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.level != 5) lastAttackTime = Time.time;
        if (playerScript.level == 5 && Time.time - lastAttackTime > attackInterval)
        {
            hasShot = false;
            animador.SetTrigger("AttackTrigger");
            lastAttackTime = Time.time;
        }

        if (attackCount == 2 && !hasShot && Time.time - lastAttackTime > attackTime) {
            hasShot = true;
            Vector3 spawnPosition = player.transform.position;
            var rangeAttack = (GameObject)Instantiate(
            rangeAttackPrefab,
            spawnPosition,
            transform.rotation);
            rangeAttack.transform.parent = bulletHelper.transform;
            attackCount = (attackCount + 1)%3;
            StartCoroutine(CheckPlayerContact(rangeAttack));
        }
        else if (playerScript.level == 5 && !hasShot && Time.time - lastAttackTime > attackTime) {
            hasShot = true;
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
            var bullet = (GameObject)Instantiate(
            bulletPrefab,
            spawnPosition,
            transform.rotation);
            bullet.transform.parent = bulletHelper.transform;
            attackCount = (attackCount + 1)%3;
        }
    }

    IEnumerator CheckPlayerContact(GameObject rangeAttack)
    {
        yield return new WaitForSeconds(1.0f);

        Vector3 spawnPosition = rangeAttack.transform.position + new Vector3(0, 1, 0);
        var boomAttack = (GameObject)Instantiate(
            boomPrefab,
            spawnPosition,
            rangeAttack.transform.rotation);
            boomAttack.transform.parent = bulletHelper.transform;

        Vector3 playerPosition = player.transform.position;

        float distance = Vector3.Distance(playerPosition, spawnPosition);

        if (distance <= radius)
        {
            livesPlayerScript.LoseLife();
        }

        Destroy(rangeAttack);
        Destroy(boomAttack, 0.5f);
    }

    void ActivateShield()
    {
    }
}
