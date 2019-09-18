using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerHitSound, spellHitSound, flameHitSound, enemy1DeathSound, enemy2DeathSound, swordSound, mineSound, grenadeSound, flameSound, spellSound, missileSound, bossHit, laugh, chaos, anything;
    static AudioSource audioSrc;



    // Start is called before the first frame update
    void Start()
    {
        playerHitSound = Resources.Load<AudioClip>("playerhit");
        spellHitSound = Resources.Load<AudioClip>("spellhit");
        flameHitSound = Resources.Load<AudioClip>("flamehit");
        enemy1DeathSound = Resources.Load<AudioClip>("enemydeath1");
        enemy2DeathSound = Resources.Load<AudioClip>("enemydeath2");
        swordSound = Resources.Load<AudioClip>("swordswing");
        mineSound = Resources.Load<AudioClip>("mine");
        grenadeSound = Resources.Load<AudioClip>("explosion");
        flameSound = Resources.Load<AudioClip>("flame");
        spellSound = Resources.Load<AudioClip>("spell");
        missileSound = Resources.Load<AudioClip>("missile");
        bossHit = Resources.Load<AudioClip>("bossHit");
        laugh = Resources.Load<AudioClip>("laugh");
        chaos = Resources.Load<AudioClip>("chaoschaos");
        anything = Resources.Load<AudioClip>("icandoanything");

        audioSrc = GetComponent<AudioSource> ();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        audioSrc.pitch = Random.Range(0.9f, 1.0f);
        switch (clip)
        {
            case "playerhit":
                audioSrc.PlayOneShot(playerHitSound);
                break;
            case "spellhit":
                audioSrc.PlayOneShot(spellHitSound);
                break;
            case "flamehit":
                audioSrc.PlayOneShot(flameHitSound);
                break;
            case "enemydeath1":
                audioSrc.PlayOneShot(enemy1DeathSound);
                break;
            case "enemydeath2":
                audioSrc.PlayOneShot(enemy2DeathSound);
                break;
            case "swordswing":
                audioSrc.PlayOneShot(swordSound);
                break;
            case "mine":
                audioSrc.PlayOneShot(mineSound);
                break;
            case "explosion":
                audioSrc.PlayOneShot(grenadeSound);
                break;
            case "flame":
                audioSrc.PlayOneShot(flameSound);
                break;
            case "spell":
                audioSrc.PlayOneShot(spellSound);
                break;
            case "missile":
                audioSrc.PlayOneShot(missileSound);
                break;
            case "bossHit":
                audioSrc.PlayOneShot(bossHit);
                break;
            case "laugh":
                audioSrc.PlayOneShot(laugh);
                break;
            case "chaoschaos":
                audioSrc.PlayOneShot(chaos);
                break;
            case "icandoanything":
                audioSrc.PlayOneShot(anything);
                break;
        }
    }
}
