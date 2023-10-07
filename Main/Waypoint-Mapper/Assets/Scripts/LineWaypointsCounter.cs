using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class LineWaypointsCounter : MonoBehaviour
{
    [Header("Inner Line")]
    public GameObject innerLineHolder;
    public TMP_Text innerLineText;
    [ReadOnly]
    public int innerLineTotalCount;

    [Header("Outer Line")]
    public GameObject outerLineHolder;
    public TMP_Text outerLineText;
    [ReadOnly]
    public int outerLineTotalCount;



    public Tuple<int,int> CalculateTotalWaypoints(string type = "start") 
    {
        int innerLineTotalCount = 0;
        int outerLineTotalCount = 0;

        if (innerLineHolder != null) {
            int childCount = innerLineHolder.transform.childCount;

            for (int i = 0; i < childCount; i++) {
                GameObject child = innerLineHolder.transform.GetChild(i).gameObject;
                GameObject waypoints = child.transform.Find("waypoints").gameObject;
                innerLineTotalCount += waypoints.transform.childCount;
            }
        }

        if (outerLineHolder != null) {
            int childCount = outerLineHolder.transform.childCount;

            for (int i = 0; i < childCount; i++) {
                GameObject child = outerLineHolder.transform.GetChild(i).gameObject;
                GameObject waypoints = child.transform.Find("waypoints").gameObject;
                outerLineTotalCount += waypoints.transform.childCount;
            }
        }

        if (type != "update") {
            Debug.Log("Inner Line Total Count: " + innerLineTotalCount);
            Debug.Log("Outer Line Total Count: " + outerLineTotalCount);
        }
        
        return new Tuple<int, int>(innerLineTotalCount, outerLineTotalCount);
    }

    private void setText(Tuple <int, int> waypointsCount, TMP_Text innerText, TMP_Text outerText) 
    {
        innerText.text = innerLineTotalCount.ToString();
        outerText.text = outerLineTotalCount.ToString();
    }

    private void Start()
    {
        Tuple <int, int> waypointsCount = CalculateTotalWaypoints();
        setText(waypointsCount, innerLineText, outerLineText);
    }

    private void Update()
    {
        Tuple <int, int> waypointsCount = CalculateTotalWaypoints("update");
        setText(waypointsCount, innerLineText, outerLineText);
    }
}
