using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    Vector3 startDirection;

    MovePlayer movePlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Store starting direction of the player with respect to the axis of the level
        startDirection = player.transform.position - player.transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();

        movePlayer = player.GetComponent<MovePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Compute current direction
        Vector3 currentDirection = player.transform.position - player.transform.parent.position;
        currentDirection.y = 0.0f;
        currentDirection.Normalize();
        // Change orientation of the camera pivot to match the player's
        Quaternion orientation;
        if ((startDirection - currentDirection).magnitude < 1e-3)
            orientation = Quaternion.AngleAxis(0.0f, Vector3.up);
        else if ((startDirection + currentDirection).magnitude < 1e-3)
            orientation = Quaternion.AngleAxis(180.0f, Vector3.up);
        else
            orientation = Quaternion.FromToRotation(startDirection, currentDirection);
        transform.parent.rotation = orientation;

        int dif;
        if (movePlayer.level == 5) {
            dif = 2;
        } else {
            dif = -1;
        }
        // Make the camera follow the player's y position
        Vector3 cameraPosition = transform.parent.position;
        cameraPosition.y = player.transform.position.y + dif;
        transform.parent.position = cameraPosition;
    }
}
