using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesPlayer : MonoBehaviour
{
    public int lives = 3;

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
            // Lógica si el jugador aún tiene vidas restantes
            
        }
    }

    // Método que se llama cuando el jugador colisiona con un enemigo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseLife();
        }
    }
}
