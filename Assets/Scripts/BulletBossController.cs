using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBossController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject impactEffect;
    public float movementSpeed;
    Vector3 startDirection;
    
    public float lifespan;

    private MoveBoss bossScript;
    private LifeEnemy lifeBoss;
    private MovePlayer movePlayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject boss = GameObject.Find("Boss");
        if (boss != null)
        {
            bossScript = boss.GetComponent<MoveBoss>();
            lifeBoss = boss.GetComponent<LifeEnemy>();
        }

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            movePlayer = player.GetComponent<MovePlayer>();
        }

        Destroy(gameObject, lifespan);
    }

    void FixedUpdate()
    {
        if (lifeBoss.health > 0) {
            Vector3 direction, target;

            Vector3 position = transform.position;

            direction = bossScript.transform.forward;
            direction.y = 0;
            target = position + direction * movementSpeed * Time.deltaTime;
            rb.MovePosition(target);
        }
        else {
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        LivesPlayer livesPlayer = collision.gameObject.GetComponent<LivesPlayer>();
        if (livesPlayer != null && movePlayer.canTakeDamage)
        {
            livesPlayer.LoseLife(10);
        }

        var impact = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impact, 0.5f);

        Destroy(gameObject);
    }
}
