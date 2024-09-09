using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class NewWindow : EditorWindow
{
    [MenuItem("Window/New Window")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(NewWindow));
    }

    void OnGUI()
    {

        
        if (GUILayout.Button("Message 1"))
        {
            Debug.Log("Message 1");
        }

        if (GUILayout.Button("Message 2"))
        {
            Debug.Log("Message 2");
        }

        // The actual window code goes hereq
    }
}
