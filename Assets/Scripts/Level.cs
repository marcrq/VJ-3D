using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private bool isActive;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isActive = !isActive;

            if (isActive)
                Time.timeScale = 0;

            else 
                Time.timeScale = 1;
            
            pauseMenu.SetActive(isActive);
            
        }
    }
}
