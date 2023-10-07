using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class WaypointsCounter : MonoBehaviour
{
    [ReadOnly]
    public int waypointsCount;
    private GameObject waypointsHolder;



    private void OnValidate()
    {
        waypointsHolder = transform.parent.Find("waypoints").gameObject;
        waypointsCount = waypointsHolder.transform.childCount;
    }

    public void ManualUpdate() 
    {
        waypointsCount = waypointsHolder.transform.childCount;
        Debug.Log("Waypoints count updated!");
    }
}
