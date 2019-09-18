using System.Collections;
using UnityEngine;


public class PistolBullet : MonoBehaviour
{
    private CircleCollider2D CC2D;
    public float pistolBulletLifeTime = 1.5f;
    public int pistolBulletDamage = 16;

    
    private void Start()
    {
        SoundManagerScript.PlaySound("spell");

        CC2D = GetComponent<CircleCollider2D>();
        Destroy(gameObject, pistolBulletLifeTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "NoCollideProjectile" || col.gameObject.tag == "Water")
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(pistolBulletDamage);
            Destroy(this.gameObject, 0f);
            if (col.gameObject.name == "BossTest")
            {
                SoundManagerScript.PlaySound("bossHit");
            }
            else
            {
                SoundManagerScript.PlaySound("spellhit");
            }
           
        }
        if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "CaveWall" || col.gameObject.tag == "CaveUnbreakableWall")
        {
            
            Destroy(this.gameObject, 0f);
        }
    }
}
