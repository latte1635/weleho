using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewArea : MonoBehaviour
{

    public string levelToLoad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (FindObjectOfType<CaveGen>().GetCurrentFloor() < 10)
            {
                FindObjectOfType<CaveGen>().GenerateMap(1);
            }
            else
            {
                SceneManager.LoadScene(2);
                other.transform.position = new Vector3(0, -6, 0);

            }
        }
    }

}
