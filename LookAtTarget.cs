using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {
    public float speed = 3f;
    public float borderThickness = 10f;
    public Vector3 limit;
    public int screenSystem = 1;
    public Vector3 rotater;
    public float speedR = 30f;
    [Tooltip("This is the object that the script's game object will look at by default")]
    public GameObject defaultTarget; // the default target that the camera should look at

    [Tooltip("This is the object that the script's game object is currently look at based on the player clicking on a gameObject")]
    public GameObject currentTarget; // the target that the camera should look at

    // Start happens once at the beginning of playing. This is a great place to setup the behavior for this gameObject
	void Start () {
		if (defaultTarget == null) 
		{
            defaultTarget = this.gameObject;
			Debug.Log ("defaultTarget target not specified. Defaulting to parent GameObject");
		}

        if (currentTarget == null)
        {
            currentTarget = this.gameObject;
            Debug.Log("currentTarget target not specified. Defaulting to parent GameObject");
        }
        Debug.Log("Left-Click to select a target to focus the camera on. Right-click to focus on the sun. Click 'c' quickly to change to Arrow Keys");
    }

    // Update is called once per frame
    // For clarity, Update happens constantly as your game is running
    void Update()
    {
        if (Input.GetKey("z")) rotater = Vector3.down;
        else if (Input.GetKey("x")) rotater = Vector3.up;
        else rotater = Vector3.zero;
        transform.Rotate(rotater * speedR * Time.deltaTime);
        // if primary mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // determine the ray from the camera to the mousePosition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // cast a ray to see if it hits any gameObjects
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            // if there are hits
            if (hits.Length > 0)
            {
                // get the first object hit
                RaycastHit hit = hits[0];
                currentTarget = hit.collider.gameObject;
                Debug.Log("defaultTarget changed to " + currentTarget.name);
            }
        }
        else if (Input.GetMouseButtonDown(1)) // if the second mouse button is pressed
        {
            currentTarget = defaultTarget;
            Debug.Log("defaultTarget changed to " + currentTarget.name);
        }
        Vector3 pos = transform.position;
        if (screenSystem == 1)
        {
            // if a currentTarget is set, then look at it
            if (currentTarget != null)
            {
                // transform here refers to the attached gameobject this script is on.
                // the LookAt function makes a transform point it's Z axis towards another point in space
                // In this case it is pointing towards the target.transform
                transform.LookAt(currentTarget.transform);
            }
            else // reset the look at back to the default
            {
                currentTarget = defaultTarget;
                Debug.Log("defaultTarget changed to " + currentTarget.name);
            }
            if (Input.GetKey("c"))
            {
                screenSystem = 2;
                //currentTarget = null;
                Debug.Log("Use Arrow Keys (or 'a', 'd', 'w', 's') to move the camera. Use 'z', 'x' to rotate the camera horizontally. Can still click an object to plan the camera's next target focus. Click 'c' quickly to change to Watch Targets");
            }
        } else
        {
            if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))// || Input.mousePosition.y >= Screen.height - borderThickness)
            {
                pos.z += speed * Time.deltaTime;
            }
            if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))// || Input.mousePosition.y <= borderThickness)
            {
                pos.z -= speed * Time.deltaTime;
            }
            if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))// || Input.mousePosition.x >= Screen.width - borderThickness)
            {
                pos.x += speed * Time.deltaTime;
            }
            if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))// || Input.mousePosition.x <= borderThickness)
            {
                pos.x -= speed * Time.deltaTime;
            }
            pos.x = Mathf.Clamp(pos.x, -limit.x, limit.x);
            pos.z = Mathf.Clamp(pos.z, -limit.z, limit.z);
            transform.position = pos;
            if (Input.GetKey("c"))
            {
                screenSystem = 1;
                Debug.Log("Left-Click to select a target to focus the camera on. Right-click to focus on the sun. Click 'c' quickly to change to Arrow Keys");
            }
        }
    }
}
