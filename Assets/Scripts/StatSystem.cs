using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    Vector3 namePos;

    void Start()
    {
        maxHealth = health;
        
        HealthUI.text = health.ToString();
        HealthBar.fillAmount = health / maxHealth;
        
    }

    void Update()
    {
        if (gameObject.tag != "Player" && gameObject.name != "BossTest")
        {
            if (gameObject.name == "Enemy3")
            {
                namePos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 64;
            }
            else
            {
                namePos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 48;
            }
            Vector3 healthPos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;
            Vector3 HealthBarPos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;
            Vector3 BarBGPos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;

            NameUI.rectTransform.position = namePos;
            HealthUI.rectTransform.position = healthPos;
            HealthBar.rectTransform.position = HealthBarPos;
            BarBG.rectTransform.position = BarBGPos;
        }
        if(gameObject.name == "Enemy3")
        {
            
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if(gameObject.name == "BossTest")
        {
            int randomInt = Random.Range(0, 2);
            switch (randomInt)
            {
                case 0:
                    DropPotion(manaPotion, 50f);
                    break;
                case 1:
                    DropPotion(healthPotion, 30f);
                    break;
            }
        }

        HealthUI.text = health.ToString();
        HealthBar.fillAmount = health / maxHealth;
        
        if(health <= 0)
        {
            if (manaPotion != null && gameObject.tag != "Player" && gameObject.name != "BossTest")
            {
                int randomInt = Random.Range(0, 2);
                switch(randomInt)
                {
                    case 0:
                        DropPotion(manaPotion, 101f);
                        break;
                    case 1:
                        DropPotion(healthPotion, 101f);
                        break;
                }
                
            }
            Destroy(gameObject);
            if (gameObject.tag == "Player")
            {
                DeathManager.Death();
                MusicManager.PlaySound("youdied");
            }
            if (gameObject.tag == "Enemy")
            {
                if (gameObject.name == "BossTest")
                {
                    WinManager.Win();
                    MusicManager.PlaySound("win");
                }
                else
                {
                    SoundManagerScript.PlaySound("enemydeath1");
                }
            }
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
            GameObject itemInstance = Instantiate(lootItem, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0f), Quaternion.identity);
            FindObjectOfType<CaveGen>().clones.Add(itemInstance);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (gameObject.tag == "Player")
        {
            if (coll.gameObject.tag == "ManaPotion")
            {
                GetComponent<Shooting>().RegenMana(200);
                Destroy(coll.gameObject);
            }
            if (coll.gameObject.tag == "HpPotion")
            {
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
}
