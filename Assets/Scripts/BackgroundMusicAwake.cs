using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicAwake : MonoBehaviour
{
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    public static BackgroundMusicAwake instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
