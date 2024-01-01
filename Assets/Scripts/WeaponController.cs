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

    void Start()
    {
        assaultRiflePBR.SetActive(false);
        pistolPBR.SetActive(true);
        gun = pistolPBR.transform;
        currentBullets = maxBullets;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CambiarArma();
        }
        if (Input.GetKeyDown(KeyCode.F) && currentBullets > 0)
        {
            animador.SetTrigger("ShootTrigger");
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animador.SetTrigger("ReloadTrigger");
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
            bulletScript.lifespan = 2f;
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