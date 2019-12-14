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
    [Tooltip("Number of characters that can be displayed")]
    public int LogBuffer = 5000;
    [Tooltip("Log Height on Buttom Screen")]
    public int LogHeight = 250;
    [Tooltip("No show on build version")]
    public bool OnlyInEditor = false;

    private bool exitClicked = false;
    private bool ShowLog = true;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        myLog = output + "\n" + myLog;
        if (stack.Length > 0 && ShowStack)
        {
            myLog = stack + "\n" + myLog;
        }
        if (myLog.Length > LogBuffer)
        {
            myLog = myLog.Substring(0, LogBuffer);

        }

        scrollPosition = new Vector2(scrollPosition.x, 0);
    }

    void OnGUI()
    {
        if (!Application.isEditor || ! OnlyInEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
        {
            if (GUI.Button(new Rect(new Rect(10, Screen.height - LogHeight - 40, 100, 40)), "L"))
                exitClicked = true;

            if (ShowLog) {

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
            GUILayout.TextArea(myLog);

            // End the scrollview we began above.
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            }

            if (exitClicked)
            {
                ShowLog = !ShowLog;
                exitClicked = false;
            }

        }
    }
    //#endif
}