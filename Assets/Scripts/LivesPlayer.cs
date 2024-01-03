using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesPlayer : MonoBehaviour
{
    public int lives = 3;
    public LayerMask defaultLayer;
    public LayerMask enemyLayer;

    public Animator animador;

    MovePlayer playerScript;

    public void Start() {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<MovePlayer>();
        }

        enemyLayer = LayerMask.NameToLayer("Enemy");
        defaultLayer = LayerMask.NameToLayer("Player");
    }

    public void LoseLife()
    {
        lives--; // Resta una vida
        Debug.Log("Vidas restantes: " + lives);

        if (lives <= 0)
        {
            // Lógica para el Game Over, por ejemplo:
            Debug.Log("Game Over");

            GameOver();
        }
        else
        {
            Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
            animador.SetTrigger("hitTrigger");
            StartCoroutine(IgnoreCollision());
        }
    }

    public void GameOver() {
        animador.SetTrigger("deathTrigger");

        playerScript.canMove = false;
        StartCoroutine(GoToGameOver());
    }

    IEnumerator GoToGameOver() {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator IgnoreCollision()
    {
        Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
        yield return new WaitForSeconds(2);
        Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, false);
    }
}
