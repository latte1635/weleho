using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject playerPrefab;
    public static bool IsGameOver = false;
    public float deathTimer;
    private float startdeathTimer;

    // Start is called before the first frame update
    void Start()
    {
        startdeathTimer = deathTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameOver == true)
        {
            gameOver.SetActive(true);
            if (startdeathTimer <= 0)
            {
                gameOver.SetActive(false);
                startdeathTimer = deathTimer;
                IsGameOver = false;
                Application.Quit();

            }
            else
            {
                startdeathTimer -= Time.deltaTime;
            }
        }

    }

    public static void Death()
    {
        IsGameOver = true;
    }

}
