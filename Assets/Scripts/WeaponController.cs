using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject pistolPBR;
    public GameObject assaultRiflePBR;
    public Animator animador;

    private Transform gun;

    public int maxBullets;
    public int totalBullets;
    public int currentBullets;
    public GameObject bulletPrefab;
    public GameObject bulletHelper;
    public bool isShooting;
    public bool isReloading;

    public MovePlayer movePlayer;

    bool hasShot;
    float lastShootTime;
    float shootTime;
    float shootCooldown;
    float lastReloadTime;
    float reloadCooldown;

    public AudioClip rifleSound;
    public AudioClip pistolSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        assaultRiflePBR.SetActive(false);
        pistolPBR.SetActive(true);
        gun = pistolPBR.transform;
        currentBullets = maxBullets;

        lastShootTime = -1.0f;
        shootCooldown = 0.5f;
        shootTime = 0.3f;

        lastReloadTime = -1.0f;
        reloadCooldown = 0.5f;
        isShooting = false;
        hasShot = false;
        isReloading = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CambiarArma();
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            ReloadMax();
        }
        if (isShooting && Time.time - lastShootTime > shootCooldown) {
            isShooting = false;
            hasShot = false;
        }
        if (isReloading && Time.time - lastReloadTime > reloadCooldown) {
            isReloading = false;
        }
        if (!isReloading && !isShooting && !movePlayer.isDashing && Input.GetKeyDown(KeyCode.F) && currentBullets > 0)
        {
            if (pistolPBR.activeSelf) {
                if (pistolSound != null)
                {
                    audioSource.PlayOneShot(pistolSound);
                }
            }
            else {
                if (rifleSound != null)
                {
                    audioSource.PlayOneShot(rifleSound);
                }
            }
            isShooting = true;
            animador.SetTrigger("ShootTrigger");
            lastShootTime = Time.time;
        }

        if (isShooting && !hasShot && Time.time - lastShootTime > shootTime) {
            hasShot = true;
            Shoot();
        }

        if (!isShooting && !isReloading && !movePlayer.isDashing && Input.GetKeyDown(KeyCode.R) && currentBullets != maxBullets)
        {
            if (reloadSound != null)
            {
                audioSource.PlayOneShot(reloadSound);
            }
            isReloading = true;
            animador.SetTrigger("ReloadTrigger");
            lastReloadTime = Time.time;
            Reload();
        }
    }

    public void CambiarArma()
    {
        pistolPBR.SetActive(!pistolPBR.activeSelf);
        assaultRiflePBR.SetActive(!assaultRiflePBR.activeSelf);

        if (pistolPBR.activeSelf)
        {
            animador.SetTrigger("PistolTrigger");
            gun = pistolPBR.transform;
        }
        else
        {
            animador.SetTrigger("RifleTrigger");
            gun = assaultRiflePBR.transform;
        }
    }

    void Shoot()
    {
        currentBullets--;
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            gun.position,
            gun.rotation);
        bullet.transform.parent = bulletHelper.transform;

        var bulletScript = bullet.GetComponent<BulletController>();

        if (pistolPBR.activeSelf)
        {
            if (movePlayer.level == 5) {
                bulletScript.lifespan = Mathf.Infinity;
            } else {
                bulletScript.lifespan = 0.3f;
            }
            bulletScript.damage = 50;
        }
        else if (assaultRiflePBR.activeSelf)
        {
            bulletScript.lifespan = Mathf.Infinity;
            bulletScript.damage = 30;
        }
    }

    void Reload()
    {
        int bulletsToReload = maxBullets - currentBullets;
        if (totalBullets >= bulletsToReload)
        {
            currentBullets = maxBullets;
            totalBullets -= bulletsToReload;
        }
        else
        {
            currentBullets += totalBullets;
            totalBullets = 0;
        }
    }

    void ReloadMax()
    {
        currentBullets = maxBullets;
    }
}