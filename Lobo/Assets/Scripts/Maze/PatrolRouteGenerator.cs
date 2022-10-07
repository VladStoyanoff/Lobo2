using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRouteGenerator : MonoBehaviour
{
    [SerializeField] Transform waypointPrefab;
    [SerializeField] Transform patrolRoutePrefab;
    List<Transform> waypoints = new List<Transform>();

    void Awake()
    {
        // Assign Patrol Route
        var allNodes = FindObjectOfType<MazeGenerator>().GetMazeNodesList();
        var numberOfWaypoints = 10;
        var patrolRoute = Instantiate(patrolRoutePrefab);
        for (int i = 0; i < numberOfWaypoints; i++)
        {
            var randomNode = allNodes[Random.Range(0, allNodes.Count)];
            var waypoint = Instantiate(waypointPrefab, randomNode.GetMazeNodePosition(), Quaternion.identity, patrolRoute.transform);
            waypoints.Add(waypoint);
        }
    }

    public List<Transform> GetWaypointsList() => waypoints;
}