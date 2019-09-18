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

    [Header("Manacosts")]
    public static int swordManaCost = 0;
    public static int pistolManaCost = 13;
    public static int rifleManaCost = 16;
    public static int flamerManaCost = 25;
    public static int grenadeManaCost = 60;
    public static int sniperManaCost = 10;
    int mineManaCost = 20;

    [Header("Rates of fire")]
    public static float swordROF = .02f;
    public static float pistolROF = .4f;
    public static float rifleROF = .2f;
    public static float flamerROF = .05f;
    public static float grenadeROF = 2f;
    public static float sniperROF = .2f;

    [Header("Projectile Speed")]
    public static float swordSpeed = 10000f;
    public static float pistolSpeed = 9000f;
    public static float rifleSpeed = .6f;
    public static float flamerSpeed = 6000f;
    public static float grenadeSpeed = 3000f;
    public static float sniperSpeed = 30f;

    [Header("Hold Down Mouse1?")]
    public static bool swordAuto = false;
    public static bool pistolAuto = true;
    public static bool rifleAuto = true;
    public static bool flamerAuto = true;
    public static bool grenadeAuto = false;
    public static bool sniperAuto = false;

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
    public LineRenderer lr;

    

    public GameObject caveFloor;

    //Stores rate of fire of current weapon, tells when weapon can be shot again
    private float nextFire;

    //Used lists here to store most weapon data, easily accessible with the index. (Could possibly be made into an array, but this works too)
    string[] weaponName = new string[] { "Sword", "Magic Bolt", "Magic Missile", "Flames", "Magical Death Potion", "WIP raycast spell" };
    int[] manaCost = new int[] { swordManaCost, pistolManaCost, rifleManaCost, flamerManaCost, grenadeManaCost, sniperManaCost };
    float[] rof = new float[] { swordROF, pistolROF, rifleROF, flamerROF, grenadeROF, sniperROF };
    float[] speed = new float[] { swordSpeed, pistolSpeed, rifleSpeed, flamerSpeed, grenadeSpeed, sniperSpeed };
    bool[] holdDown = new bool[] { swordAuto, pistolAuto, rifleAuto, flamerAuto, grenadeAuto, sniperAuto };

    //GameObjects could not be put into a list while simultaneously also be able to be defined in the editor, had to create separate GameObject variable "currentWeaponGO".

    int selectedWeaponIndex;
    GameObject currentWeaponGO;
    
    void Start()
    {

        //Sword selected as the default weapon, player starts with it in hand. Also updates UI.
        selectedWeaponIndex = 0;
        currentWeaponGO = SwordPrefab;
        WeaponNameUI.text = weaponName[0];
        ManaUI.text = "1000";
        Debug.Log("Started");
        anim = GetComponent<Animator>();
        Mana.transform.position = ManaBG.transform.position;//new Vector3(0, 10, 0);
        maxMana = currentMana;
        ManaBar.fillAmount = currentMana / maxMana;
    }

    void Update()
    {
        //Mana.text = currentMana.ToString();
        //Determine if semi automatic or fully automatic
        if (holdDown[selectedWeaponIndex] == false)
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            RefreshMana();
        }

        //Select weapon with number keys, update UI.
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeaponIndex = 0;
            currentWeaponGO = SwordPrefab;
            WeaponNameUI.text = weaponName[selectedWeaponIndex];
            ManaUI.text = "";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeaponIndex = 1;
            currentWeaponGO = PistolBulletPrefab;
            WeaponNameUI.text = weaponName[selectedWeaponIndex];
            ManaUI.text = manaCost[selectedWeaponIndex].ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeaponIndex = 2;
            currentWeaponGO = RifleBulletPrefab;
            WeaponNameUI.text = weaponName[selectedWeaponIndex];
            ManaUI.text = manaCost[selectedWeaponIndex].ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedWeaponIndex = 3;
            currentWeaponGO = FlamerFlamePrefab;
            WeaponNameUI.text = weaponName[selectedWeaponIndex];
            ManaUI.text = manaCost[selectedWeaponIndex].ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedWeaponIndex = 4;
            currentWeaponGO = GrenadePrefab;
            WeaponNameUI.text = weaponName[selectedWeaponIndex];
            ManaUI.text = manaCost[selectedWeaponIndex].ToString();
        }
        /*if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedWeaponIndex = 5;
            currentWeaponGO = SniperPrefab;
            WeaponNameUI.text = weaponName[selectedWeaponIndex];
            ManaUI.text = manaCost[selectedWeaponIndex].ToString();
        }*/
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (DistanceCheck(4f) && currentMana >= mineManaCost) {
                if (GameObject.FindGameObjectWithTag("CaveGen").GetComponent<CaveGen>().MineWallTile())
                {
                    currentMana -= mineManaCost;
                }
                Mana.text = currentMana.ToString();
                ManaBar.fillAmount = currentMana / maxMana;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
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
        else
        {
            return false;
        }
    }

    void Shoot()
    {
        nextFire = Time.time + rof[selectedWeaponIndex];

        if (currentMana >= manaCost[selectedWeaponIndex])
        {
            float distanceFromShooter = 12f;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Find mouse direction related to player
            Vector3 direction = (worldMousePos - transform.position).normalized;
            //direction.Normalize();

            //calculate projectile sprite orientation when created
            float angle = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 4) * Mathf.Rad2Deg;
            if (selectedWeaponIndex != 5)
            {
                //Create projectile and define its orientation
                GameObject projectile = Instantiate(currentWeaponGO, transform.position + direction * distanceFromShooter, Quaternion.identity);

                //if not flame or grenade
                if (selectedWeaponIndex != 3 && selectedWeaponIndex != 4)
                {
                    //rotate projectiles that have to be rotated
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                //Give projectile its directional velocity
                projectile.transform.GetComponent<Rigidbody2D>().AddForce(direction * speed[selectedWeaponIndex]);
            }

            //Shooting a raycast spell
            if (selectedWeaponIndex == 5)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction);
                //if raycast hits something
                if (hitInfo)
                {
                    Debug.Log(hitInfo.transform.gameObject.name);
                    //Destroy(hitInfo.transform.gameObject);
                    lr.SetPosition(0, transform.position + direction * 1000f);
                    lr.SetPosition(1, hitInfo.point);
                }
                else
                {
                    lr.SetPosition(0, transform.position + direction * 1000f);
                    lr.SetPosition(1, transform.position + direction * 10000f);
                }
            }
            //If not sword, because sword has no ammo
            
            
                //substract manacost of used spell from current mana and update UI
                currentMana -= manaCost[selectedWeaponIndex];
                Mana.text = currentMana.ToString();

            Debug.Log(currentMana + "___" + maxMana);
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
            //Debug.Log(currentMana + " / " + maxMana);
            ManaBar.fillAmount = currentMana / maxMana;
            
            yield return new WaitForSeconds(tickSpeed);
        }
        
    }

    public void RegenMana(int amount)
    {
        StartCoroutine(ManaRegen(1, amount, 0.01f));
    }

    
}


