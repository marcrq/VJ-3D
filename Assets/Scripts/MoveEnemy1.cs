using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy1 : MonoBehaviour
{
    public float movementSpeed;
    public bool movingRight = true;
    Vector3 startDirection;
    CharacterController charControl;

    void Start()
    {
        charControl = GetComponent<CharacterController>();

        // Store starting direction of the player with respect to the axis of the level
        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();

        movingRight = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle;
        Vector3 direction, target;

        Vector3 position = transform.position; // Declare 'position' variable here

        angle = movementSpeed * Time.deltaTime; // Use 'movementSpeed' instead of 'rotationSpeed'
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

        // Change orientation of enemy accordingly
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
