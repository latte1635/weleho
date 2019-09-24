using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public float moveSpeed = .01f;

    private Rigidbody2D myRb;


    [Header("Shooting")] public float nextFire;

    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float rof;

    public Transform target { get; set; }

    void Awake()
    {
        gameObject.GetComponent<StatSystem>().SetCharacterType(3);
    }
    
    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        FollowTarget();
        if (target != null && Time.time > nextFire) Shoot();
    }

    private void FollowTarget()
    {
        if (target != null)
            myRb.velocity = new Vector3((target.position.x - transform.position.x) * moveSpeed,
                (target.position.y - transform.position.y) * moveSpeed, 0f);
        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
        else
            myRb.velocity = Vector3.zero;
    }


    private void Shoot()
    {
        nextFire = Time.time + rof;

        var direction = target.position - transform.position;
        direction.Normalize();

        //calculate projectile sprite orientation when created
        var angle = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 4) * Mathf.Rad2Deg;


        //Create projectile and define its orientation
        var projectile = Instantiate(projectilePrefab, transform.position + direction, Quaternion.identity);

        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Give projectile its directional velocity
        projectile.transform.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }
}