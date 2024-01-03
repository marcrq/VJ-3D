using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesPlayer : MonoBehaviour
{
    public int lives = 3;
    public LayerMask defaultLayer;
    public LayerMask enemyLayer;

    public Animator animador;

    public void LoseLife()
    {
        lives--; // Resta una vida
        Debug.Log("Vidas restantes: " + lives);

        if (lives <= 0)
        {
            // Lógica para el Game Over, por ejemplo:
            Debug.Log("Game Over");
            // Aquí podrías reiniciar el nivel, mostrar un mensaje de game over, etc.
        }
        else
        {
            animador.SetTrigger("hitTrigger");
            StartCoroutine(IgnoreCollision());
        }
    }

    IEnumerator IgnoreCollision()
    {
        Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
        yield return new WaitForSeconds(2);
        Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseLife();
        }
    }
}
