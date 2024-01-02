using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeEnemy : MonoBehaviour
{
    public int shield;
    public int health;

    public Animator animador;

    //public Slider shieldSlider;
    //public Slider healthSlider;

    void Start()
    {
        //shieldSlider.maxValue = shield;
        //shieldSlider.value = shield;
        //healthSlider.maxValue = health;
        //healthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        animador.SetTrigger("HitTrigger");
        if (shield > 0)
        {
            shield -= damage;
            if (shield < 0) shield = 0;
            //shieldSlider.value = shield;
        }
        else
        {
            health -= damage;
            if (health <= 0) {
                Destroy(gameObject);
            }
            //healthSlider.value = health;
        }
    }
}
