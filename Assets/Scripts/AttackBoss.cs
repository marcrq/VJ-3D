using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject rangeAttackPrefab;
    public GameObject boomPrefab;

    public float attackInterval = 5.0f;
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

    public AudioClip boomSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        player = GameObject.Find("Player");
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
            if (attackCount == 2) {
                animador.SetTrigger("AttackTrigger2");
            }
            else {
                animador.SetTrigger("AttackTrigger");
            }
            lastAttackTime = Time.time;
        }

        if (attackCount == 2 && !hasShot && Time.time - lastAttackTime > attackTime) {
            hasShot = true;
            Vector3 spawnPosition = player.transform.position;
            spawnPosition.y += 0.1f;
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

        if (boomSound != null)
        {
            audioSource.PlayOneShot(boomSound);
        }

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
            livesPlayerScript.LoseLife(20);
        }

        Destroy(rangeAttack);
        Destroy(boomAttack, 0.5f);
    }

    void ActivateShield()
    {
    }

    void Shoot()
    {
    }

    void PlayStep()
    {
    }
}
