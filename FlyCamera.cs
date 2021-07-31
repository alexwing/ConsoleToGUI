using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public. 
    Made simple to use (drag and drop, done) for regular keyboard layout  
    WASD : basic movement
    SHIFT : Makes camera accelerate
    SPACE : Moves camera on X and Z axis only.  So camera doesn't gain any height
	*/




    [Tooltip("The terrain to follow")]
    public Terrain anchorToTerrain;
    private float marginToTerrain = 0;
    [Tooltip("The min height of the terrain to fly over")]
    public float minHeighToTerrain = 20f;
    [Tooltip("The max height of the terrain to fly over")]
    public float maxHeighToTerrain = 120f;


    [Tooltip("Regular speed")]
    public float mainSpeed = 2f;
    [Tooltip("Rotation speed")]
    public float rotationSpeed = 20f;
    [Tooltip(" Multiplied by how long shift is held.  Basically running.")]

    public float shiftAdd = 125f;
    [Tooltip("Maximum speed when holdin gshift")]
    public float maxShift = 100f;
    [Tooltip("How sensitive it with mouse")]
    public float camSens = 0.25f;
    private float totalRun = 1f;
    public static bool lockMovement = false;


    private void Awake()
    {
        marginToTerrain = ((maxHeighToTerrain - minHeighToTerrain) * 0.5f) + minHeighToTerrain;

    }

    private void Update()
    {
        Cursor.visible = lockMovement;
        Cursor.lockState = lockMovement ? CursorLockMode.None : CursorLockMode.Locked;

        if (!lockMovement)
        {
            // Mouse camera angle.  
            float h = Input.GetAxis("Mouse X") * rotationSpeed;
            float v = Input.GetAxis("Mouse Y") * rotationSpeed;
            Vector3 delta = new Vector3(h, v, 0f);

            delta = new Vector3(-delta.y * camSens, delta.x * camSens, 0f);
            delta = new Vector3(transform.eulerAngles.x + delta.x, transform.eulerAngles.y + delta.y, 0f);
            transform.eulerAngles = delta;

            // Keyboard commands

            Vector3 p = GetBaseInput();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift * mainSpeed);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift * mainSpeed);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift * mainSpeed);
            }
            else
            {
                totalRun = Mathf.Clamp(mainSpeed * 0.5f, 1f, 1000f);
                p = p * totalRun;
            }

            p = p * Time.deltaTime;
            transform.Translate(p);

        }

        if (anchorToTerrain)
        {
            Vector3 newPosition = transform.position;

            // Anchor to terrain.
            float height = anchorToTerrain.SampleHeight(this.transform.position);
            //block movement to terrain limits x and z
            newPosition.x = Mathf.Clamp(newPosition.x, anchorToTerrain.transform.position.x, anchorToTerrain.transform.position.x + anchorToTerrain.terrainData.size.x);
            newPosition.z = Mathf.Clamp(newPosition.z, anchorToTerrain.transform.position.z, anchorToTerrain.transform.position.z + anchorToTerrain.terrainData.size.z);
            this.transform.position = new Vector3(newPosition.x, height + marginToTerrain, newPosition.z);

            //mouse whell change marginToTerrain
            marginToTerrain -= Input.GetAxis("Mouse ScrollWheel") * mainSpeed;

            if (minHeighToTerrain > marginToTerrain)
            {
                marginToTerrain = minHeighToTerrain;
            }
            if (marginToTerrain > maxHeighToTerrain)
            {
                marginToTerrain = maxHeighToTerrain;
            }
        }


        //lock movement with mouse click
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonUp(1))
        {
            lockMovement = !lockMovement;
        }

    }

    private Vector3 GetBaseInput()
    {
        // Returns the basic values, if it's 0 than it's not active.

        Vector3 p_Velocity = new Vector3();

        if (!lockMovement)
        {
            if (Input.GetKey(KeyCode.W)) p_Velocity += Vector3.forward;
            if (Input.GetKey(KeyCode.S)) p_Velocity += Vector3.back;
            if (Input.GetKey(KeyCode.A)) p_Velocity += Vector3.left;
            if (Input.GetKey(KeyCode.D)) p_Velocity += Vector3.right;
        }

        return p_Velocity;
    }
}
