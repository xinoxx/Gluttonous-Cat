using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip eatClip;

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    public void PlayEatAudio()
    {
        audioSource.PlayOneShot(eatClip);
    }
}
