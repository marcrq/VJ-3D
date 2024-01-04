using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public void OnClickResumeButton()
    {
        pauseMenu.SetActive(false);
    }

    public void OnClickExitButton()
    {
        SceneManager.LoadScene("Menu");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
