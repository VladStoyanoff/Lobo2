using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    PatrolRouteGenerator patrolRouteGenerator;
    PlayerController player;
    NavMeshAgent navMeshAgent;

    [SerializeField] GameObject bulletPrefab;

    int waypointIndex = 0;

    const float WAYPOINT_WIDTH = .3f;
    const int CHASE_RADIUS = 1;

    float timeSinceLastShot = Mathf.Infinity;
    const int FIRE_RATE = 1;
    const float BULLET_SPEED = 2f;

    Vector3 waypointPosition;
    bool isNotInRangeOfPlayer;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        patrolRouteGenerator = GetComponent<PatrolRouteGenerator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        navMeshAgent.updateUpAxis = navMeshAgent.updateRotation = false;
        transform.eulerAngles = Vector3.zero;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        PatrolBehaviour();
        AttackBehaviour();
    }

    void PatrolBehaviour()
    {
        AssignPatrolAndCurrentWaypoint(out waypointPosition, out var waypointList);
        TryApproachingNextWaypoint(waypointPosition);
        TryRestartPatrolRoute(waypointList);
    }

    void AttackBehaviour()
    {
        if (player == null) return;
        CheckIfPlayerIsInRange(out isNotInRangeOfPlayer);
        if (isNotInRangeOfPlayer) return;
    }

    void AssignPatrolAndCurrentWaypoint(out Vector3 waypointPosition, out List<Transform> waypointList)
    {
        var list = patrolRouteGenerator.GetWaypointsList();
        var waypoint = list[waypointIndex].position;
        waypointList = list;
        waypointPosition = waypoint;
        navMeshAgent.destination = waypointPosition;
    }

    void TryApproachingNextWaypoint(Vector3 waypointPosition)
    {
        var distanceToWaypoint = Vector3.Distance(transform.position, waypointPosition);
        if (distanceToWaypoint > WAYPOINT_WIDTH) return;
        waypointIndex++;
    }

    void TryRestartPatrolRoute(List<Transform> waypointList)
    {
        if (waypointIndex != waypointList.Count) return;
        waypointIndex = 0;
    }

    void CheckIfPlayerIsInRange(out bool isNotInRangeOfPlayer)
    {
        var distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        var check = distanceToPlayer > CHASE_RADIUS;
        isNotInRangeOfPlayer = check;
    }

    public void RotateTowards(Vector2 positionToRotateTowards)
    {
        var rotationSpeed = 1000;
        var angle = Mathf.Atan2(positionToRotateTowards.y - transform.position.y, positionToRotateTowards.x - transform.position.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }

    public void TryShoot(Vector3 bulletDirection)
    {
        if (timeSinceLastShot < FIRE_RATE) return;
        var bullet = Instantiate(bulletPrefab, gameObject.transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity);
        bullet.tag = "Enemy Bullet";
        bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * BULLET_SPEED;
        timeSinceLastShot = 0;
    }

    public Vector3 GetWaypointPosition() => waypointPosition;
    public bool GetIsNotInRangeOfPlayerBool() => isNotInRangeOfPlayer;
    public PlayerController GetPlayerController() => player;
}
