using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Transform mousepos;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    

    private static bool cameraExists;

    private void Start()
    {
        
        try
        {
            target = GameObject.FindWithTag("CameraTarget").transform;
            GetTargetPosition();
            //target.position = (GameObject.FindWithTag("Player").transform.position + Camera.main.ScreenToWorldPoint(Input.mousePosition))/2;
        }
        catch
        {
            Debug.Log("Cannot find Player, Where is he?");
        }

        //checks if camera is already in scene
        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        GetTargetPosition();        
        //Smoothly follows player
        Vector3 desiredPosition = target.position + offset;


        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    void GetTargetPosition()
    {
        target.position = new Vector3((GameObject.FindWithTag("Player").transform.position.x + Camera.main.ScreenToWorldPoint(Input.mousePosition).x) / 2, (GameObject.FindWithTag("Player").transform.position.y + Camera.main.ScreenToWorldPoint(Input.mousePosition).y) / 2, 0f);
    }
}
