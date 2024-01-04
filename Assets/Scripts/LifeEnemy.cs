using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeEnemy : MonoBehaviour
{
    public int shield;
    public int health;

    public Animator animador;

    public int shieldMid;

    public GameObject parent;
    public GameObject shield100;
    public GameObject shield50;

    public Slider shieldSlider;
    public Slider healthSlider;

    public bool isBoss;

    LivesPlayer livesPlayer;

    public AudioClip boomSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        shield100.SetActive(true);
        shield50.SetActive(false);
        shieldSlider.maxValue = shield;
        shieldSlider.value = shield;
        healthSlider.maxValue = health;
        healthSlider.value = health;

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            livesPlayer = player.GetComponent<LivesPlayer>();
        }
    }

    public void TakeDamage(int damage)
    {
        animador.SetTrigger("HitTrigger");
        if (shield > 0)
        {
            shield -= damage;
            if (shield <= shieldMid) {
                shield100.SetActive(false);
                shield50.SetActive(true);
            }
            if (shield <= 0) {
                shield = 0;
                shield50.SetActive(false);
            }
            shieldSlider.value = shield;
        }
        else
        {
            health -= damage;
            if (health <= 0) {
                if (!isBoss) {
                    Destroy(parent);
                }
                else {
                    StartCoroutine(PlayBoomSound());
                    animador.SetTrigger("deathTrigger");
                    livesPlayer.YouWin();
                    Destroy(parent, 3.0f);
                }
            }
            healthSlider.value = health;
        }
    }

    IEnumerator PlayBoomSound() {
        yield return new WaitForSeconds(3.0f);
        if (boomSound != null)
        {
            audioSource.PlayOneShot(boomSound);
        }
    }
}
