using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy2 : MonoBehaviour
{
    public float movementSpeed;
    public bool movingRight = true;
    Vector3 startDirection;
    CharacterController charControl;

    LivesPlayer livesPlayer;

    void Start()
    {
        charControl = GetComponent<CharacterController>();

        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            livesPlayer = player.GetComponent<LivesPlayer>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle;
        Vector3 direction, target;

        Vector3 position = transform.position;

        angle = movementSpeed * Time.deltaTime;
        direction = position - transform.parent.position;

        if (movingRight)
        {
            target = transform.parent.position + Quaternion.AngleAxis(-angle, Vector3.up) * direction;
            if (charControl.Move(target - position) != CollisionFlags.None)
            {
                transform.position = position;
                Physics.SyncTransforms();
                movingRight = false;
            }
        }
        else
        {
            target = transform.parent.position + Quaternion.AngleAxis(angle, Vector3.up) * direction;
            if (charControl.Move(target - position) != CollisionFlags.None)
            {
                transform.position = position;
                Physics.SyncTransforms();
                movingRight = true;
            }
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            livesPlayer.LoseLife(5);
        }
    }
}