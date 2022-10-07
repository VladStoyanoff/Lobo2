using UnityEngine;

public class BasicUnit : MonoBehaviour
{
    AIController aiController;

    void Start() 
    {
        aiController = GetComponent<AIController>();
    }

    void Update()
    {
        aiController.RotateTowards(aiController.GetWaypointPosition());
        if (aiController.GetIsNotInRangeOfPlayerBool()) return;
        aiController.TryShoot(transform.right);
    }
}
