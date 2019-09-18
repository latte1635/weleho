using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    public float moveSpeed = 1f;

    private Rigidbody2D myRb;

    private Transform target;
    public Transform Target { get => target; set => target = value; }


    [Header("Shooting")]
    public float nextFire;
    public float rof;
    public float projectileSpeed;
    public GameObject projectilePrefab;



    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
        if (target != null && Time.time > nextFire)
        {
            Shoot();
        }
    }

    void FollowTarget()
    {
        if (target != null)
        {
            myRb.velocity = new Vector3((target.position.x - transform.position.x) * moveSpeed, (target.position.y - transform.position.y) * moveSpeed, 0f);
            //transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
        }
        else
        {
            myRb.velocity = Vector3.zero;
        }
    }



    void Shoot()
    {
        nextFire = Time.time + rof;

        Vector3 direction = (target.position - transform.position);
        direction.Normalize();

        //calculate projectile sprite orientation when created
        float angle = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 4) * Mathf.Rad2Deg;


        //Create projectile and define its orientation
        GameObject projectile = Instantiate(projectilePrefab, transform.position + (Vector3)(direction * 1f), Quaternion.identity);

        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Give projectile its directional velocity
        projectile.transform.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

    }


}
