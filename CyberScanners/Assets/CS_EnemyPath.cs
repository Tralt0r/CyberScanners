using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class EnemyPath : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    public Transform GetWaypoint(int index)
    {
        if (index < waypoints.Count)
            return waypoints[index];

        return null;
    }

    public int WaypointCount()
    {
        return waypoints.Count;
    }

    void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        if (waypoints == null || waypoints.Count < 2) return;

        Handles.color = Color.cyan;

        Vector3[] points = new Vector3[waypoints.Count];
        for (int i = 0; i < waypoints.Count; i++)
        {
            points[i] = waypoints[i].position;
        }

        Handles.DrawAAPolyLine(10f, points);

        foreach (Transform wp in waypoints)
        {
            if (wp == null) continue;
            Gizmos.DrawSphere(wp.position, 0.2f);
        }
    #endif
    }
}