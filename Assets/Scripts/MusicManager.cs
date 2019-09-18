using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static AudioClip bossFight, victorySound, youdied, cardCastle;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        bossFight = Resources.Load<AudioClip>("joker");
        victorySound = Resources.Load<AudioClip>("win");
        youdied = Resources.Load<AudioClip>("youdied");
        cardCastle = Resources.Load<AudioClip>("card_castle");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "joker":
                audioSrc.Stop();
                audioSrc.loop = true;
                audioSrc.clip = bossFight;
                audioSrc.Play();
                break;
            case "win":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = victorySound;
                audioSrc.Play();
                break;
            case "youdied":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = youdied;
                audioSrc.Play();
                break;
            case "card_castle":
                audioSrc.Stop();
                audioSrc.loop = true;
                audioSrc.clip = cardCastle;
                audioSrc.Play();
                break;
        }
    }
}
