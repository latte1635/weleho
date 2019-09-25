using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StatSystem : MonoBehaviour
{
    public float health;
    private float maxHealth;
    
    public Text HealthUI;
    public Text NameUI;
    public Image HealthBar;
    
    public Image BarBG;

    public GameObject manaPotion;
    public GameObject healthPotion;

    private UInt16 characterType;
    Vector3 namePos;

    void Start()
    {
        maxHealth = health;
        
        HealthUI.text = health.ToString();
        HealthBar.fillAmount = health / maxHealth;
    }

    void Update()
    {
        if (characterType != 0 && characterType != 5)
        {
            if (characterType != 3)
                namePos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 64;
            else
                namePos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 48;
            
            Vector3 healthPos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;
            Vector3 HealthBarPos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;
            Vector3 BarBGPos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;

            NameUI.rectTransform.position = namePos;
            HealthUI.rectTransform.position = healthPos;
            HealthBar.rectTransform.position = HealthBarPos;
            BarBG.rectTransform.position = BarBGPos;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if(characterType == 5)
        {
            if (Random.Range(0, 2) == 0)     DropPotion(manaPotion, 50f);
            if (Random.Range(0, 2) == 1)     DropPotion(healthPotion, 30f);
        }

        HealthUI.text = health.ToString();
        HealthBar.fillAmount = health / maxHealth;
        
        if(health <= 0)
        {
            if (manaPotion != null && characterType != 0 && characterType != 5)
            {
                int randomInt = Random.Range(0, 2);
                if(randomInt == 0) DropPotion(manaPotion, 101f);
                if(randomInt == 1) DropPotion(healthPotion, 101f);
                 
            }
            
            if (characterType == 0)
            {
                GetComponent<PlayerController>().OnGameEnd();
                DeathManager.Death();
                MusicManager.PlaySound("youdied");
            }
            
            if (characterType > 0)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().gaStats.Mobkills++;
                Debug.Log("Mobs killed: " + GameObject.FindWithTag("Player").GetComponent<PlayerController>().gaStats.Mobkills);
                if (characterType == 5)
                {
                    GameObject.FindWithTag("Player").GetComponent<PlayerController>().gaStats.GameWon = 1;
                    WinManager.Win();
                    MusicManager.PlaySound("win");
                }
                else
                {
                    SoundManagerScript.PlaySound("enemydeath1");
                }
            }
            Destroy(gameObject);
        }
    }

    public void UpdateCharacterUI()
    {
        HealthUI.text = health.ToString();
        HealthBar.fillAmount = health / maxHealth;
    }

    public void DropPotion(GameObject lootItem, float dropChance)
    {
        float randomFloat = Random.Range(0f, 100f);
        if(dropChance > randomFloat)
        {
            GameObject itemInstance = Instantiate(lootItem, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
            FindObjectOfType<CaveGen>().clones.Add(itemInstance);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (characterType == 0)
        {
            if (coll.gameObject.CompareTag("ManaPotion"))
            {
                GetComponent<PlayerController>().gaStats.ManaPotionsCollected++;
                GetComponent<Shooting>().RegenMana(200);
                Destroy(coll.gameObject);
            }
            if (coll.gameObject.CompareTag("HpPotion"))
            {
                GetComponent<PlayerController>().gaStats.HealthPotionsCollected++;
                RegenHealth(20);
                Destroy(coll.gameObject);
            }

        }
    }

    public IEnumerator HealthRegen(int amount, int duration, float tickSpeed)
    {
        for (int i = duration; i > 0; i--)
        {
            if (health < maxHealth)
            {
                health++;
                HealthUI.text = health.ToString();
                HealthBar.fillAmount = health / maxHealth;
            }
            yield return new WaitForSeconds(tickSpeed);
        }

    }

    public void RegenHealth(int amount)
    {
        StartCoroutine(HealthRegen(1, amount, 0.01f));
    }

    public void SetCharacterType(UInt16 typeIndex)
    {
        characterType = typeIndex;
    }

    public UInt16 GetCharacterType()
    {
        return characterType;
    }
}
