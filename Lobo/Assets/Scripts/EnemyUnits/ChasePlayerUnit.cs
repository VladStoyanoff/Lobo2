using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerUnit : MonoBehaviour
{
    NavMeshAgent navMesh;
    AIController aiController;

    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        aiController = GetComponent<AIController>();
    }

    void Update()
    {
        if (aiController.GetIsNotInRangeOfPlayerBool()) return;
        navMesh.destination = aiController.GetPlayerController().transform.position;
        aiController.RotateTowards(navMesh.destination);
        aiController.TryShoot((aiController.GetPlayerController().transform.position - transform.position).normalized);
    }
}
