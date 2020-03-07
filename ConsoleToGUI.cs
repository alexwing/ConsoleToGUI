using System.IO;
using UnityEngine;

public class ConsoleToGUI : MonoBehaviour
{
    //#if !UNITY_EDITOR
    static string myLog = "";
    private string output;
    private string stack;
    private Vector2 scrollPosition;
    [Tooltip("Show/hide Stack Trace in Log")]
    public bool ShowStack = true;
    [Tooltip("Show/hide FPS Count")]
    public bool ShowFPS = true;
    [Tooltip("Number of characters that can be displayed")]
    public int LogBuffer = 5000;
    [Tooltip("Log Height on Buttom Screen")]
    public int LogHeight = 250;
    [Tooltip("No show on build version")]
    public bool OnlyInEditor = false;
    [Tooltip("Save file with log")]
    public bool SaveLogFile = true;

    public bool DisplayInUi = false;
    private bool exitClicked = false;
    [Tooltip("Log textbox show on start")]
    public bool ShowLog = true;

    public string logPath = "";
    public bool logPathInApplicationPath = true;
    private float deltaTime = 0.0f;


    [Tooltip("Font log size")]
    public int FontLogSize = 25;


    private void Start()
    {
        if (logPathInApplicationPath)
        {
            logPath = Utils.GetAppPath() + "/Log";
        }
        else
        {
            logPath = Utils.GetPath() + "/Log";
        }

        EvaluatePath();
    }
    private void EvaluatePath()
    {

        if (!Directory.Exists(logPath))
            Directory.CreateDirectory(logPath);

        Debug.Log("Log path in: " + logPath);

    }

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }
    void Update()
    {
        if(ShowFPS && ShowLog)
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }
    public void WriteLog(string logString)
    {

        if (logPath != "" && SaveLogFile)
        {
            TextWriter tw = new StreamWriter(logPath + "/log.txt", true);
            tw.WriteLine(logString);
            tw.Close();
        }
    }
    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;

        // write a line of text to the file


        // close the stream


        myLog = output + "\n" + myLog;
        if (stack.Length > 0 && ShowStack)
        {
            myLog = stack + "\n" + myLog;
            WriteLog(System.DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + output + "\n" + stack + "\n");


        }
        else
        {
            WriteLog(System.DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + output + "\n");

        }
        if (myLog.Length > LogBuffer)
        {
            myLog = myLog.Substring(0, LogBuffer);

        }





        scrollPosition = new Vector2(scrollPosition.x, 0);
    }

    void OnGUI()
    {
        if (DisplayInUi)
        {
            if (!Application.isEditor || !OnlyInEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                if (GUI.Button(new Rect(new Rect(10, Screen.height - LogHeight - 40, 100, 40)), "L"))
                    exitClicked = true;

                if (ShowLog)
                {

                    if (GUI.Button(new Rect(new Rect(130, Screen.height - LogHeight - 40, 100, 40)), "S"))
                    {
                        ShowStack = !ShowStack;
                    }
                    if (GUI.Button(new Rect(new Rect(250, Screen.height - LogHeight - 40, 100, 40)), "C"))
                    {
                        myLog = "";
                    }


                       

                    // we want to place the TextArea in a particular location - use BeginArea and provide Rect
                    GUILayout.BeginArea(new Rect(10, Screen.height - LogHeight, Screen.width - 20, Screen.height - 10));
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width - 20), GUILayout.Height(LogHeight));

                    // We just add a single label to go inside the scroll view. Note how the
                    // scrollbars will work correctly with wordwrap.
                    GUIStyle textStyle = new GUIStyle(GUI.skin.textArea);
                    textStyle.fontSize = FontLogSize;

                    GUILayout.TextArea(myLog, textStyle);

                    // End the scrollview we began above.
                    GUILayout.EndScrollView();
                    GUILayout.EndArea();

                    if (ShowFPS)
                    {
                        string FPS = string.Format("{0:0.0} ms ({1:0.} fps)", deltaTime * 1000.0f, 1.0f / deltaTime);
                        GUI.Label(new Rect(new Rect(370, Screen.height - LogHeight - 40, 250, 40)), FPS, textStyle);
                    }


                }

                if (exitClicked)
                {
                    ShowLog = !ShowLog;
                    exitClicked = false;
                }

            }
        }
    }
    //#endif
}
