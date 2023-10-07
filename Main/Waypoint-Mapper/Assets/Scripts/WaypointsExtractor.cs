using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class WaypointsExtractor : MonoBehaviour
{
    public GameObject TrackList;
    private bool isValid;
    private GameObject activeTrack;

    public string savePath;
    private string savePathDefault = "Assets/Waypoints/";
    public string fileName;


    
    private bool checkValid(GameObject TrackList)
    {
        int childCount = TrackList.transform.childCount;
        int isActiveCount = 0;
        
        for (int i = 0; i < childCount; i++) {
            GameObject child = TrackList.transform.GetChild(i).gameObject;
            if (child.activeSelf) {
                isActiveCount += 1;
            }
        }

        if (isActiveCount == 1) {
            return true;
        }

        return false;
    }

    private GameObject findActiveTrack(GameObject TrackList)
    {
        int childCount = TrackList.transform.childCount;
            
        for (int i = 0; i < childCount; i++) {
            GameObject child = TrackList.transform.GetChild(i).gameObject;
            if (child.activeSelf) {
                return child;
            }
        }
        
        return null;
    }

    private bool checkWaypointsValid(GameObject innerLine_obj, GameObject outerLine_obj)
    {
        int innerChildCount = innerLine_obj.transform.childCount;
        int outerChildCount = outerLine_obj.transform.childCount;

        if (innerChildCount != outerChildCount) {
            return false;
        }

        for (int i = 0; i < innerChildCount; i++) {
            GameObject innerChild = innerLine_obj.transform.GetChild(i).gameObject;
            GameObject outerChild = outerLine_obj.transform.GetChild(i).gameObject;

            if (innerChild.transform.Find("waypoints").gameObject.transform.childCount != 
                outerChild.transform.Find("waypoints").gameObject.transform.childCount) {
                    return false;
            }
        }

        return true;
    }

    private Vector3 calculateCenterPoint(Vector3 innerPos, Vector3 outerPos)
    {
        float x = (innerPos.x + outerPos.x) / 2;
        float y = (innerPos.y + outerPos.y) / 2;
        float z = (innerPos.z + outerPos.z) / 2;

        return new Vector3(x, y, z);
    }

    private int recordWaypoints(int rowIndex, GameObject innerWaypoints_obj, GameObject outerWaypoints_obj,
                                Vector3[] innerWaypoints, Vector3[] outerWaypoints, Vector3[] centerWaypoints)
    {
        int childCount = innerWaypoints_obj.transform.childCount;

        for (int i = 0; i < childCount; i++) {
            GameObject innerChild = innerWaypoints_obj.transform.GetChild(i).gameObject;
            GameObject outerChild = outerWaypoints_obj.transform.GetChild(i).gameObject;

            Vector3 innerPos = innerChild.transform.position;
            Vector3 outerPos = outerChild.transform.position;

            innerWaypoints[rowIndex] = innerPos;
            outerWaypoints[rowIndex] = outerPos;
            centerWaypoints[rowIndex] = calculateCenterPoint(innerPos, outerPos);

            rowIndex += 1;
        }

        return rowIndex;
    }

    private void writeToCsv(string path, int totalWaypointsCount,
                            Vector3[] innerWaypoints, Vector3[] outerWaypoints, Vector3[] centerWaypoints)
    {
        Debug.Log("Writing waypoints to \"" + path + "\"...");

        System.IO.StreamWriter file = new System.IO.StreamWriter(path);

        string headers = "inner_x,inner_y,inner_z," +
                         "outer_x,outer_y,outer_z," +
                         "center_x,center_y,center_z";

        file.WriteLine(headers);

        for (int i = 0; i < totalWaypointsCount; i++) {
            string line = innerWaypoints[i].x + "," + innerWaypoints[i].y + "," + innerWaypoints[i].z + ","
                          + outerWaypoints[i].x + "," + outerWaypoints[i].y + "," + outerWaypoints[i].z + ","
                          + centerWaypoints[i].x + "," + centerWaypoints[i].y + "," + centerWaypoints[i].z;
            file.WriteLine(line);
        }
        
        file.Close();
    }

    public void ExtractWaypoints()
    {
        isValid = checkValid(TrackList);
        activeTrack = findActiveTrack(TrackList);

        if (isValid) {
            Debug.Log("Extracting waypoints of \"" + activeTrack.name + "\"...");
            
            GameObject innerLine_obj = activeTrack.transform.Find("waypoints").gameObject.transform.Find("inner line").gameObject;
            GameObject outerLine_obj = activeTrack.transform.Find("waypoints").gameObject.transform.Find("outer line").gameObject;

            if (checkWaypointsValid(innerLine_obj, outerLine_obj)) {
                LineWaypointsCounter script = activeTrack.GetComponent<LineWaypointsCounter>();
                Tuple<int,int> totalWaypointsCount = script.CalculateTotalWaypoints();

                Vector3[] innerWaypoints = new Vector3[totalWaypointsCount.Item1];
                Vector3[] outerWaypoints = new Vector3[totalWaypointsCount.Item1];
                Vector3[] centerWaypoints = new Vector3[totalWaypointsCount.Item1];

                int pointCount = innerLine_obj.transform.childCount;

                int rowIndex = 0;
                for (int i = 0; i < pointCount; i++) {
                    GameObject innerChild = innerLine_obj.transform.GetChild(i).gameObject;
                    GameObject outerChild = outerLine_obj.transform.GetChild(i).gameObject;

                    GameObject innerWaypoints_obj = innerChild.transform.Find("waypoints").gameObject;
                    GameObject outerWaypoints_obj = outerChild.transform.Find("waypoints").gameObject;

                    rowIndex = recordWaypoints(rowIndex, innerWaypoints_obj, outerWaypoints_obj,
                                               innerWaypoints, outerWaypoints, centerWaypoints);
                }

                if (savePath.Trim() == "") {
                    savePath = savePathDefault;
                }

                if (savePath[savePath.Length - 1] != '/' && savePath[savePath.Length - 1] != '\\') {
                    savePath += '/';
                }

                if (fileName.Trim() == "") {
                    fileName = activeTrack.name;
                }

                string path = savePath + fileName + ".csv";
                writeToCsv(path, totalWaypointsCount.Item1,
                           innerWaypoints, outerWaypoints, centerWaypoints);

                Debug.Log("\"" + activeTrack.name + "\" waypoints extracted successfully to \"" + path + "\"!");
            } else {
                Debug.LogError("Invalid - Different number of inner and outer waypoints!");
            }
        } else {
            Debug.LogError("Invalid - There was more than 1 active track.");
        }
    }
}