using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    
    
    private StatSystem statSystem;

    public int bossDamage;


    // Start is called before the first frame update
    void Start()
    {

        statSystem = GetComponent<StatSystem>();

        statSystem.UpdateCharacterUI();

    }

    // Update is called once per frame
    void Update()
    {
        statSystem.UpdateCharacterUI();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SoundManagerScript.PlaySound("playerhit");
            col.gameObject.GetComponent<StatSystem>().TakeDamage(bossDamage);

        }
    }



}
