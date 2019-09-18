using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource bang;

    void Start()
    {
        bang = GetComponent<AudioSource>();
    }

    public void SetClip(AudioClip sound)
    {
        bang.clip = sound;
    }

    public void PlayBang()
    {
        bang.Play();
    }
}
