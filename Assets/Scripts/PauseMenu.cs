using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Level level;

    public void OnClickResumeButton()
    {
        level.isActive = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnClickExitButton()
    {
        level.isActive = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject LevelGO = GameObject.Find("Level");
        if (LevelGO != null)
        {
            level = LevelGO.GetComponent<Level>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
