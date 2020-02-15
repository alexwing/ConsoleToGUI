using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
   // public GameObject TemporalObject;
    float deltaTime = 0.0f;
    public Text text;
    public SphereCollider Mesh;
    private PortalTeleporter puertaCollider;
    private int layerMask;
    void Start()
    {
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        layerMask = LayerMask.GetMask("Puerta");
        
        //Application.targetFrameRate = 100;
    }
    void Update()
    {
        
       // Debug.Log(Vector3.Dot(transform.position, TemporalObject.transform.position));
        //Debug.Log(Vector3.Distance(transform.position, TemporalObject.transform.position));
        RaycastHit hit = new RaycastHit();
        Ray ray;
        if (Physics.Raycast(Camera.main.transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity,layerMask))
        {
            Debug.Log(hit.collider.name);
            Debug.DrawRay(Camera.main.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.collider.gameObject.tag == "Puerta")
            {
                Debug.Log("da");
                puertaCollider=hit.collider.gameObject.GetComponent<PortalTeleporter>();
               // puertaCollider.enabled = true;
            }
            else
            {
                hit.collider.gameObject.layer = 2;
            }
        }
        else
        {
               // puertaCollider.enabled = false;
        }
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }
    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
        
    }
}