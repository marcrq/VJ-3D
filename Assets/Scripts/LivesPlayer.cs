using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LivesPlayer : MonoBehaviour
{
    public int lives = 100;
    public LayerMask defaultLayer;
    public LayerMask enemyLayer;

    public Slider healthSlider;
    public TextMeshProUGUI health;

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

        healthSlider.maxValue = lives;
        healthSlider.value = lives;
        health.text = lives.ToString();

        StartCoroutine(GameOverAfterTwoMinutes());
    }

    public void LoseLife(int damage)
    {
        lives -= damage; // Resta vida
        if (lives >= 0)
        {
            healthSlider.value = lives;
            health.text = lives.ToString();
        }

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

    public void YouWin() {
        animador.SetTrigger("winTrigger");

        playerScript.canMove = false;
        StartCoroutine(GoToCredits());
    }

    IEnumerator GoToGameOver() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator GoToCredits() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Credits");
    }

    IEnumerator IgnoreCollision()
    {
        Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
        yield return new WaitForSeconds(2);
        Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, false);
    }

    IEnumerator GameOverAfterTwoMinutes() {
        yield return new WaitForSeconds(120);
        GameOver();
    }
}
