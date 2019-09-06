using UnityEngine;

    public class ConsoleToGUI : MonoBehaviour
    {
        //#if !UNITY_EDITOR
        static string myLog = "";
        private string output;
        private string stack;
        private Vector2 scrollPosition;
        public bool ShowStack = true;
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
            if (myLog.Length > 5000)
            {
                myLog = myLog.Substring(0, 4000);
               
            }
            
            scrollPosition = new Vector2(scrollPosition.x, Mathf.Infinity);
    }

        void OnGUI()
        {
            //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {

            // we want to place the TextArea in a particular location - use BeginArea and provide Rect
            GUILayout.BeginArea(new Rect(10, Screen.height - 250, Screen.width - 10, Screen.height - 10));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width - 10), GUILayout.Height(250));

            // We just add a single label to go inside the scroll view. Note how the
            // scrollbars will work correctly with wordwrap.
            GUILayout.TextArea(myLog);

            // End the scrollview we began above.
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        }
        //#endif
    }

