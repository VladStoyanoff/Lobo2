using UnityEngine;
using UnityEngine.AI;

public class RamPlayerUnit : MonoBehaviour
{
    PlayerController player;
    NavMeshAgent navMeshAgent;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        navMeshAgent.updateUpAxis = navMeshAgent.updateRotation = false;
        transform.eulerAngles = Vector3.zero;
    }

    void Update()
    {
        if (player == null) return;
        navMeshAgent.destination = player.transform.position;
    }
}
