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

    float lastShootTime;
    float shootCooldown;
    float lastReloadTime;
    float reloadCooldown;

    void Start()
    {
        assaultRiflePBR.SetActive(false);
        pistolPBR.SetActive(true);
        gun = pistolPBR.transform;
        currentBullets = maxBullets;

        lastShootTime = -1.0f;
        shootCooldown = 0.5f;

        lastReloadTime = -1.0f;
        reloadCooldown = 0.5f;
        isShooting = false;
        isReloading = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CambiarArma();
        }
        if (isShooting && Time.time - lastShootTime > shootCooldown) {
            isShooting = false;
        }
        if (isReloading && Time.time - lastReloadTime > reloadCooldown) {
            isReloading = false;
        }
        if (!isReloading && !isShooting && !movePlayer.isDashing && Input.GetKeyDown(KeyCode.F) && currentBullets > 0)
        {
            isShooting = true;
            animador.SetTrigger("ShootTrigger");
            lastShootTime = Time.time;
            Shoot();
        }

        if (!isShooting && !isReloading && !movePlayer.isDashing && Input.GetKeyDown(KeyCode.R) && currentBullets == maxBullets)
        {
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
        Debug.Log(gun.position);
        currentBullets--;
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            gun.position,
            gun.rotation);
        bullet.transform.parent = bulletHelper.transform;

        var bulletScript = bullet.GetComponent<BulletController>();

        if (pistolPBR.activeSelf)
        {
            bulletScript.lifespan = 0.3f;
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
}