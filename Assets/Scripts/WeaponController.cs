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
    public int currentBulletsPistol;
    public int currentBulletsRifle;
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
    public bool hasRifle;

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
        currentBulletsPistol = currentBulletsRifle = maxBullets;

        lastShootTime = -1.0f;
        shootCooldown = 0.5f;
        shootTime = 0.3f;

        lastReloadTime = -1.0f;
        reloadCooldown = 0.5f;
        isShooting = false;
        hasShot = false;
        isReloading = false;
        hasRifle = false;
    }

    void Update()
    {
        if (movePlayer.canMove) {
            if (hasRifle && Input.GetKeyDown(KeyCode.C))
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
            if (!isReloading && !isShooting && !movePlayer.isDashing && Input.GetKeyDown(KeyCode.F) && 
                ((currentBulletsPistol > 0 && pistolPBR.activeSelf) || (currentBulletsRifle > 0 && assaultRiflePBR.activeSelf)))
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

            if (!isShooting && !isReloading && !movePlayer.isDashing && Input.GetKeyDown(KeyCode.R) &&
                ((currentBulletsPistol != maxBullets && pistolPBR.activeSelf) || (currentBulletsRifle != maxBullets && assaultRiflePBR.activeSelf)))
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
        if (pistolPBR.activeSelf) {
            currentBulletsPistol--;
        } else {
            currentBulletsRifle--;
        }
        
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
        if (pistolPBR.activeSelf) {
            int bulletsToReload = maxBullets - currentBulletsPistol;
            if (totalBullets >= bulletsToReload)
            {

                currentBulletsPistol = maxBullets;
                totalBullets -= bulletsToReload;
            }
            else
            {
                currentBulletsPistol += totalBullets;
                totalBullets = 0;
            }
        }
        else {
            int bulletsToReload = maxBullets - currentBulletsRifle;
            if (totalBullets >= bulletsToReload)
            {

                currentBulletsRifle = maxBullets;
                totalBullets -= bulletsToReload;
            }
            else
            {
                currentBulletsRifle += totalBullets;
                totalBullets = 0;
            }
        }
    }

    void ReloadMax()
    {
        currentBulletsRifle = currentBulletsPistol = maxBullets;
    }
}