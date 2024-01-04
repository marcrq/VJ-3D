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

    public GameObject UI;
    public GameObject youWin;

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
        StartCoroutine(WinUI());
        StartCoroutine(GoToCredits());
    }

    IEnumerator WinUI()
    {
        yield return new WaitForSeconds(3);
        youWin.SetActive(true);
        UI.SetActive(false);
        playerScript.canMove = false;
    }

    IEnumerator GoToGameOver() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator GoToCredits() {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("Credits");
    }

    IEnumerator IgnoreCollision()
    {
        Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, true);
        yield return new WaitForSeconds(2);
        Physics.IgnoreLayerCollision(defaultLayer, enemyLayer, false);
    }
}
