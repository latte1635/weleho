using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    private GameObject player;
    public GameObject pressSpace;
    public static bool GameIsOver = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (GameIsOver == true)
        {
            pressSpace.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Application.Quit();
            }
        }

    }

    public static void Win()
    {
        GameIsOver = true;
    }

}
