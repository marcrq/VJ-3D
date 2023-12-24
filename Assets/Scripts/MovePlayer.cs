using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    Animator animator;
    public LayerMask enemyLayer;
    public LayerMask defaultLayer;
    public float rotationSpeed, jumpSpeed, gravity;
    public bool running_right, running_left, last_running_left;
    
    bool isOut;
    Vector3 startDirection;
    float speedY;

    bool isDashing, canTakeDamage;
    float dashDuration, dashTimer;
    float lastDashTime;
    float dashCooldown;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // Store starting direction of the player with respect to the axis of the level
        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();

        speedY = 0;
        isOut = true;
        running_left = false;
        running_right = false;
        last_running_left = false;

        isDashing = false;
        canTakeDamage = true;
        dashDuration = 0.5f;
        dashTimer = 0.0f;

        enemyLayer = LayerMask.NameToLayer("Enemy");
        defaultLayer = gameObject.layer;

        dashCooldown = 2.0f;
        lastDashTime = -dashCooldown;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (canTakeDamage) {
                canTakeDamage = false;
                Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
            }
            else {
                canTakeDamage = true;
                Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CharacterController charControl = GetComponent<CharacterController>();
        Vector3 position;

        // Dash
        if (Input.GetKey(KeyCode.L) && !isDashing && Time.time - lastDashTime > dashCooldown && canTakeDamage)
        {
            isDashing = true;
            animator.SetBool("dashing", true);
            dashTimer = 0.0f;

            Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
            lastDashTime = Time.time;
        }

        if (isDashing)
        {
            dashTimer += Time.deltaTime;

            float angle;
            Vector3 direction, target;

            position = transform.position;
            angle = rotationSpeed * Time.deltaTime * 2;
            direction = position - transform.parent.position;

            if (last_running_left) {
                target = transform.parent.position + Quaternion.AngleAxis(angle, Vector3.up) * direction;
                if (charControl.Move(target - position) != CollisionFlags.None)
                {
                    transform.position = position;
                    Physics.SyncTransforms();
                }
            } else {
                target = transform.parent.position + Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                if (charControl.Move(target - position) != CollisionFlags.None)
                {
                    transform.position = position;
                    Physics.SyncTransforms();
                }
            }

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                animator.SetBool("dashing", false);
                Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, false);
            }
        }
        else {
            running_left = running_right = false;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                Vector3 direction, target, up;

                position = transform.position;
                direction = position - transform.parent.position;
                up = new Vector3(0.0f, transform.position.y, 0.0f);

                if (Input.GetKey(KeyCode.W) && isOut)
                {
                    target = Vector3.MoveTowards(transform.position, up, 2f);
                    isOut = false;
                    if (charControl.Move(target - position) != CollisionFlags.None)
                    {
                        transform.position = target;
                        Physics.SyncTransforms();
                    }
                }
                if (Input.GetKey(KeyCode.S) && !isOut)
                {
                    target = Vector3.MoveTowards(transform.position, up, -2f);
                    isOut = true;
                    if (charControl.Move(target - position) != CollisionFlags.None)
                    {
                        transform.position = target;
                        Physics.SyncTransforms();
                    }
                }
            }


            // Left-right movement
            if (Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D))
            {
                float angle;
                Vector3 direction, target;

                position = transform.position;
                angle = rotationSpeed * Time.deltaTime;
                direction = position - transform.parent.position;
                if (Input.GetKey(KeyCode.A))
                {
                    running_left = true;
                    last_running_left = true;
                    target = transform.parent.position + Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    if (charControl.Move(target - position) != CollisionFlags.None)
                    {
                        transform.position = position;
                        Physics.SyncTransforms();
                    }
                }
                if (Input.GetKey(KeyCode.D))
                {
                    running_right = true;
                    last_running_left = false;
                    target = transform.parent.position + Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    if (charControl.Move(target - position) != CollisionFlags.None)
                    {
                        transform.position = position;
                        Physics.SyncTransforms();
                    }
                }
            }
            animator.SetBool("running", running_right ^ running_left);
        }

        // Correct orientation of player
        // Compute current direction
        Vector3 currentDirection = transform.position - transform.parent.position;
        currentDirection.y = 0.0f;
        currentDirection.Normalize();

        // Change orientation of player accordingly
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

        // Check for left movement and adjust orientation
        if (last_running_left)
        {
            orientation *= Quaternion.AngleAxis(180.0f, Vector3.up);
        }

        transform.rotation = orientation;

        // Apply up-down movement
        position = transform.position;
        if (charControl.Move(speedY * Time.deltaTime * Vector3.up) != CollisionFlags.None)
        {
            transform.position = position;
            Physics.SyncTransforms();
        }
        if (charControl.isGrounded)
        {
            if (speedY < 0.0f) {
                speedY = 0.0f;
                animator.SetBool("jumping", false);
            }
            if (Input.GetKey(KeyCode.Space) && !isDashing) {
                speedY = jumpSpeed;
                animator.SetBool("jumping", true);
            }
        }
        else
            speedY -= gravity * Time.deltaTime;
    }
}


