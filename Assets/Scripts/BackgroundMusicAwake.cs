using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicAwake : MonoBehaviour
{
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
        DontDestroyOnLoad(gameObject);
    }
}
