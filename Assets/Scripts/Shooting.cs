using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public float currentMana = 1000;
    public float maxMana = 1000;
    public GameObject playerPrefab;

    private Animator anim;
    
    struct Weapon
    {
        public string Name { get; }
        public int ManaCost{ get; }
        public float RateOfFire{ get; }
        public float ProjectileSpeed { get; }
        public bool HoldButton { get; }
        public Weapon(string name, int manaCost, float rateOfFire, float projectileSpeed, bool holdButton)
        {
            Name = name;
            ManaCost = manaCost;
            RateOfFire = rateOfFire;
            ProjectileSpeed = projectileSpeed;
            HoldButton = holdButton;
        }
    }
    
    private ArrayList weapons;
    int mineManaCost = 20;
    
    [Header("Prefabs")]
    public GameObject SwordPrefab;
    public GameObject PistolBulletPrefab;
    public GameObject RifleBulletPrefab;
    public GameObject FlamerFlamePrefab;
    public GameObject GrenadePrefab;
    public GameObject SniperPrefab;
    
    [Header("User Interface")]
    public Text ManaUI;
    public Text WeaponNameUI;
    public Text Mana;
    public Image ManaBar;
    public Image ManaBG;

    [Header("Line Renderer")]
    
    public GameObject caveFloor;

    private float nextFire;

    int selectedWeaponIndex;
    GameObject currentWeaponGO;

    private void Awake()
    {
        
        weapons = new ArrayList();
        weapons.Add(new Weapon("Sword", 0, .02f, 10000, false));
        weapons.Add(new Weapon("Magic Bolt", 13, .4f, 9000, true));
        weapons.Add(new Weapon("Magic Missile", 16, .2f, .6f, true));
        weapons.Add(new Weapon("Flames", 25, .05f, 6000, true)); 
        weapons.Add(new Weapon("Explosive Potion", 60, 2.0f, 3000, false));
        bool testi = ((Weapon)weapons[0]).HoldButton;
    }

    void Start()
    {
        selectedWeaponIndex = 0;
        currentWeaponGO = SwordPrefab;
        WeaponNameUI.text = ((Weapon)weapons[selectedWeaponIndex]).Name;
        ManaUI.text = "1000";
        Debug.Log("Started");
        anim = GetComponent<Animator>();
        Mana.transform.position = ManaBG.transform.position;
        maxMana = currentMana;
        ManaBar.fillAmount = currentMana / maxMana;
    }

    void Update()
    {
        if (((Weapon)weapons[selectedWeaponIndex]).HoldButton == false)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextFire)
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire)
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.O) && Input.GetKeyDown(KeyCode.L))
        {
            RefreshMana();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeaponIndex = 0;
            currentWeaponGO = SwordPrefab;
            WeaponNameUI.text = ((Weapon)weapons[selectedWeaponIndex]).Name;
            ManaUI.text = "";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeaponIndex = 1;
            currentWeaponGO = PistolBulletPrefab;
            WeaponNameUI.text = ((Weapon)weapons[selectedWeaponIndex]).Name;
            ManaUI.text = ((Weapon)weapons[selectedWeaponIndex]).ManaCost.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeaponIndex = 2;
            currentWeaponGO = RifleBulletPrefab;
            WeaponNameUI.text = ((Weapon)weapons[selectedWeaponIndex]).Name;
            ManaUI.text = ((Weapon)weapons[selectedWeaponIndex]).ManaCost.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedWeaponIndex = 3;
            currentWeaponGO = FlamerFlamePrefab;
            WeaponNameUI.text = ((Weapon)weapons[selectedWeaponIndex]).Name;
            ManaUI.text = ((Weapon)weapons[selectedWeaponIndex]).ManaCost.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedWeaponIndex = 4;
            currentWeaponGO = GrenadePrefab;
            WeaponNameUI.text = ((Weapon)weapons[selectedWeaponIndex]).Name;
            ManaUI.text = ((Weapon)weapons[selectedWeaponIndex]).ManaCost.ToString();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (DistanceCheck(4f) && currentMana >= mineManaCost) {
                if (GameObject.FindGameObjectWithTag("CaveGen").GetComponent<CaveGen>().MineWallTile())
                {
                    currentMana -= mineManaCost;
                }
                Mana.text = currentMana.ToString();
                ManaBar.fillAmount = currentMana / maxMana;
                GetComponent<PlayerController>().gaStats.TimesDug++;
            }
        }
        if (Input.GetKeyDown(KeyCode.P) && Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(ManaRegen(1, 200, 0.01f));
        }
    }

    void RefreshMana()
    {
        currentMana = 1000;
        Mana.text = currentMana.ToString();
        ManaBar.fillAmount = currentMana / maxMana;
    }

    bool DistanceCheck(double range)
    {
        double distance;
        
        distance = Math.Sqrt(Math.Pow(Math.Abs(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - GameObject.FindWithTag("Player").transform.position.x), 2) + Math.Pow(Math.Abs(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - GameObject.FindWithTag("Player").transform.position.y), 2f));
        if (distance < range)
        {
            return true;
        }
        return false;
    }

    void Shoot()
    {
        if (selectedWeaponIndex == 0) GetComponent<PlayerController>().gaStats.WeaponUsedSword++;
        if (selectedWeaponIndex == 1) GetComponent<PlayerController>().gaStats.WeaponUsedPistol++;
        if (selectedWeaponIndex == 2) GetComponent<PlayerController>().gaStats.WeaponUsedRifle++;
        if (selectedWeaponIndex == 3) GetComponent<PlayerController>().gaStats.WeaponUsedFlame++;
        if (selectedWeaponIndex == 4) GetComponent<PlayerController>().gaStats.WeaponUsedGrenade++;
        
        nextFire = Time.time + ((Weapon)weapons[selectedWeaponIndex]).RateOfFire;

        if (currentMana >= ((Weapon)weapons[selectedWeaponIndex]).ManaCost)
        {
            float distanceFromShooter = 12f;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (worldMousePos - transform.position).normalized;
            float angle = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 4) * Mathf.Rad2Deg;
            if (selectedWeaponIndex != 5)
            {
                
                GameObject projectile = Instantiate(currentWeaponGO, transform.position + direction * distanceFromShooter, Quaternion.identity);
                
                if (selectedWeaponIndex != 3 && selectedWeaponIndex != 4)
                {
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                projectile.transform.GetComponent<Rigidbody2D>().AddForce(direction * ((Weapon)weapons[selectedWeaponIndex]).ProjectileSpeed);
            }
            
            currentMana -= ((Weapon)weapons[selectedWeaponIndex]).ManaCost;
            Mana.text = currentMana.ToString();
            Debug.Log("Mana: " + currentMana + ", Max mana: " + maxMana); 
            ManaBar.fillAmount = currentMana / maxMana;
        }
    }

    public IEnumerator ManaRegen(int amount, int duration, float tickSpeed)
    {
        for (int i = duration; i > 0; i--)
        {
            if (currentMana < maxMana)
            {
                currentMana++;
                Mana.text = currentMana.ToString();
            }
            ManaBar.fillAmount = currentMana / maxMana;
            yield return new WaitForSeconds(tickSpeed);
        }
    }

    public void RegenMana(int amount)
    {
        StartCoroutine(ManaRegen(1, amount, 0.01f));
    }
}


