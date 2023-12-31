using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject impactEffect;
    public float movementSpeed;
    public bool movingRight = true;
    Vector3 startDirection;
    
    public float lifespan;
    public int damage;

    private MovePlayer playerScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();
        
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<MovePlayer>();
            if (playerScript != null)
            {
                movingRight = !playerScript.last_running_left;
            }
        }

        LayerMask playerLayer = LayerMask.NameToLayer("Player");
        LayerMask islandLayer = LayerMask.NameToLayer("Island");

        Physics.IgnoreLayerCollision(gameObject.layer, playerLayer, true);
        Physics.IgnoreLayerCollision(gameObject.layer, islandLayer, true);

        Destroy(gameObject, lifespan);
    }

    void FixedUpdate()
    {
        
        Vector3 direction, target;

        Vector3 position = transform.position;

        if (playerScript.level == 5)
        {
            direction = playerScript.transform.forward;
            target = position + direction * movementSpeed * Time.deltaTime;
            rb.MovePosition(target);
        }
        else {
            float angle;
            angle = movementSpeed * Time.deltaTime;
            direction = position - transform.parent.position;

            if (movingRight)
            {
                target = transform.parent.position + Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                rb.MovePosition(new Vector3(target.x, position.y, target.z));
            }
            else
            {
                target = transform.parent.position + Quaternion.AngleAxis(angle, Vector3.up) * direction;
                rb.MovePosition(new Vector3(target.x, position.y, target.z));
            }

            Vector3 currentDirection = transform.position - transform.parent.position;
            currentDirection.y = 0.0f;
            currentDirection.Normalize();

            Quaternion orientation;
            if ((startDirection - currentDirection).magnitude < 1e-3)
            {
                orientation = Quaternion.AngleAxis(0.0f, Vector3.up);
            }
            else if ((startDirection + currentDirection).magnitude < 1e-3)
            {
                orientation = Quaternion.AngleAxis(180.0f, Vector3.up);
            }
            else
            {
                orientation = Quaternion.FromToRotation(startDirection, currentDirection);
            }

            if (!movingRight)
            {
                orientation *= Quaternion.AngleAxis(180.0f, Vector3.up);
            }
            
            Vector3 perpendicularDirection = Vector3.Cross(startDirection, Vector3.up).normalized;
            Quaternion perpendicularRotation = Quaternion.LookRotation(perpendicularDirection, Vector3.up);

            transform.rotation = orientation * perpendicularRotation;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            LifeEnemy lifeEnemy = collision.gameObject.GetComponent<LifeEnemy>();
            if (lifeEnemy != null)
            {
                lifeEnemy.TakeDamage(damage);
            }

            var impact = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(impact, 0.5f);
            Destroy(gameObject);
        }
    }
}
