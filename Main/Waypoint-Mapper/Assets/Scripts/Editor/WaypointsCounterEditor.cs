using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointsCounter))]
public class WaypointsCounterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WaypointsCounter waypointsCounter = (WaypointsCounter)target;

        if(GUILayout.Button("Manual Count Update")) {
            waypointsCounter.ManualUpdate();
        }
    }
}
