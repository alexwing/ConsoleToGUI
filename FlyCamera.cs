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

    public float mainSpeed = 2f;			// Regular speed.
	public float rotationSpeed = 20f;		// Rotation speed.
	private float shiftAdd = 25f;			// Multiplied by how long shift is held.  Basically running.
	private float maxShift = 100f;			// Maximum speed when holdin gshift.
	private float camSens = 0.25f;			// How sensitive it with mouse.
    private float totalRun = 1f;
    private bool lockMovement = false;

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
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;

            if (Input.GetKey(KeyCode.Space))
            { 
				// If player wants to move on X and Z axis only

                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else transform.Translate(p);
		}

        if (Input.GetKeyDown(KeyCode.Escape))
			lockMovement = !lockMovement;
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