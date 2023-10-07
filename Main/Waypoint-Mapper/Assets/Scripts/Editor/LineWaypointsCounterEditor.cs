using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineWaypointsCounter))]
public class LineWaypointsCounterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        LineWaypointsCounter lineWaypointsCounter = (LineWaypointsCounter)target;
        
        if(GUILayout.Button("Manual Count Update")) {
            lineWaypointsCounter.CalculateTotalWaypoints();
        }
    }
}
