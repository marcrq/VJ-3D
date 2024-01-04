using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy1 : MonoBehaviour
{
    public float movementSpeed;
    public bool movingRight;
    public float jumpSpeed;
    public float gravity;
    private float verticalSpeed;

    private CharacterController charControl;
    private Vector3 startDirection;
    private float lastJumpTime;
    private float jumpInterval;

    LivesPlayer livesPlayer;

    void Start()
    {
        charControl = GetComponent<CharacterController>();

        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();

        jumpInterval = 3.0f;
        verticalSpeed = 0.0f;
        lastJumpTime = -1.0f;

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

        if (charControl.Move(verticalSpeed * Time.deltaTime * Vector3.up) != CollisionFlags.None)
        {
            transform.position = position;
            Physics.SyncTransforms();
        }
        if (Time.time - lastJumpTime > jumpInterval && Random.value < 0.1f)
        {
            verticalSpeed = jumpSpeed;
            lastJumpTime = Time.time;
        }

        if (charControl.isGrounded)
        {
            if (verticalSpeed < 0.0f)
            {
                verticalSpeed = 0.0f;
            }
        }
        else
        {
            verticalSpeed -= gravity * Time.deltaTime;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            livesPlayer.LoseLife(5);
        }
    }
}
