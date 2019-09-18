using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour
{
    private PlayerController thePlayer;
    private CameraFollow theCamera;

    // Start is called before the first frame update
    void Start()
    {
        //moves player and camera to wanted point on the level
        thePlayer = FindObjectOfType<PlayerController>();
        thePlayer.transform.position = transform.position;

        theCamera = FindObjectOfType<CameraFollow>();
        theCamera.transform.position = new Vector3(transform.position.x, transform.position.y, theCamera.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
