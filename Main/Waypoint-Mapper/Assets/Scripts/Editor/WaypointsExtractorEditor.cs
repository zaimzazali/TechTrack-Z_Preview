using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointsExtractor))]
public class WaypointsExtractorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        WaypointsExtractor waypointsExtractor = (WaypointsExtractor)target;

        if(GUILayout.Button("Extract Waypoints")) {
            waypointsExtractor.ExtractWaypoints();
        }
    }
}
