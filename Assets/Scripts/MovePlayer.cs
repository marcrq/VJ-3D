using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    Animator animator;
    public LayerMask enemyLayer;
    public LayerMask attackLayer;
    public LayerMask defaultLayer;
    public float rotationSpeed, jumpSpeed, gravity;
    public bool running_right, running_left, last_running_left;
    
    Vector3 startDirection;
    float speedY;

    public bool isDashing;
    bool canTakeDamage;

    float dashDuration, dashTimer;
    float lastDashTime;
    float dashCooldown;

    public int level;

    public bool canMove;

    public AudioClip jumpSound;
    public AudioClip dashSound;
    private AudioSource audioSource;

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        startDirection = transform.position - transform.parent.position;
        startDirection.y = 0.0f;
        startDirection.Normalize();

        speedY = 0;
        running_left = false;
        running_right = false;
        last_running_left = false;

        isDashing = false;
        canTakeDamage = true;
        dashDuration = 0.2f;
        dashTimer = 0.0f;

        enemyLayer = LayerMask.NameToLayer("Enemy");
        attackLayer = LayerMask.NameToLayer("Attack");
        defaultLayer = gameObject.layer;

        dashCooldown = 2.0f;
        lastDashTime = -dashCooldown;
        level = 1;

        canMove = true;
    }

    void Update()
    {
        CharacterController charControl = GetComponent<CharacterController>();
        if (canMove && charControl != null && charControl.enabled) {
            if (Input.GetKeyDown(KeyCode.G)) {
                if (canTakeDamage) {
                    canTakeDamage = false;
                    Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
                    Physics.IgnoreLayerCollision(defaultLayer, attackLayer, true);
                }
                else {
                    canTakeDamage = true;
                    Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, false);
                    Physics.IgnoreLayerCollision(defaultLayer, attackLayer, false);
                }
            }

            if (Input.GetKeyDown(KeyCode.L) && !isDashing && Time.time - lastDashTime > dashCooldown && canTakeDamage)
            {
                isDashing = true;
                if (level == 5) {
                    if (last_running_left) {
                        Debug.Log("entra");
                        animator.SetBool("dashingLeft", true);
                    } else {
                        animator.SetBool("dashingRight", true);
                    }
                }
                else {
                    animator.SetBool("dashing", true);
                }
                dashTimer = 0.0f;

                Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
                lastDashTime = Time.time;

                if (dashSound != null)
                {
                    audioSource.PlayOneShot(dashSound);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CharacterController charControl = GetComponent<CharacterController>();
        if (charControl != null && charControl.enabled) {
            Vector3 position;
            if (canMove) {
                WeaponController weaponController = GetComponent<WeaponController>();

                if (!weaponController.isShooting && !weaponController.isReloading) {
                    // Dash
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
                            if (level == 5) {
                                animator.SetBool("dashingLeft", false);
                                animator.SetBool("dashingRight", false);
                            }
                            else {
                                animator.SetBool("dashing", false);
                            }
                            Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, false);
                        }
                    }
                    else {
                        running_left = running_right = false;

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
                        if (level == 5) {
                            animator.SetBool("runningLeft", running_left);
                            animator.SetBool("runningRight", running_right);
                        }
                        else {
                            animator.SetBool("running", running_right ^ running_left);
                        }
                    }
                }
            }

            // Change orientation of player accordingly
            Quaternion orientation;

            // Correct orientation of player
            // Compute current direction
            Vector3 currentDirection = transform.position - transform.parent.position;
            currentDirection.y = 0.0f;
            currentDirection.Normalize();

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
            if (level != 5 && last_running_left)
            {
                orientation *= Quaternion.AngleAxis(180.0f, Vector3.up);
            }
            if (level == 5) {
                orientation *= Quaternion.AngleAxis(270.0f, Vector3.up);
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
                if (Input.GetKey(KeyCode.Space) && !isDashing && canMove) {
                    speedY = jumpSpeed;
                    animator.SetBool("jumping", true);
                    if (jumpSound != null)
                    {
                        audioSource.PlayOneShot(jumpSound);
                    }
                }
            }
            else
                speedY -= gravity * Time.deltaTime;
        }
    }
}


