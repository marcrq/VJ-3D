using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBossController : MonoBehaviour
{
    public Rigidbody rb;
    //public GameObject impactEffect;
    public float movementSpeed;
    Vector3 startDirection;
    
    public float lifespan;

    private MoveBoss bossScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject boss = GameObject.Find("Boss");
        if (boss != null)
        {
            bossScript = boss.GetComponent<MoveBoss>();
        }

        Destroy(gameObject, lifespan);
    }

    void FixedUpdate()
    {
        Vector3 direction, target;

        Vector3 position = transform.position;

        direction = bossScript.transform.forward;
        direction.y = 0;
        target = position + direction * movementSpeed * Time.deltaTime;
        rb.MovePosition(target);
    }


    void OnCollisionEnter(Collision collision)
    {
        LivesPlayer livesPlayer = collision.gameObject.GetComponent<LivesPlayer>();
        if (livesPlayer != null)
        {
            livesPlayer.LoseLife();
        }

        // Crear un efecto de impacto cuando la bala golpea algo
        //Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
