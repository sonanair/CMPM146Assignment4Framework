using UnityEngine;

public class DistanceQuery : BehaviorTree
{
    private float maxDistance;
    private Transform target;
    private bool checkPlayer;

    public DistanceQuery(float maxDistance, Transform target = null, bool checkPlayer = false)
    {
        this.maxDistance = maxDistance;
        this.target = target;
        this.checkPlayer = checkPlayer;
    }

    public override Result Run()
    {
        if (agent == null) return Result.FAILURE;
        Transform checkTarget = checkPlayer ? GameObject.FindGameObjectWithTag("Player")?.transform : target;
        if (checkTarget == null) return Result.FAILURE;
        
        float distance = Vector3.Distance(agent.transform.position, checkTarget.position);
        return distance <= maxDistance ? Result.SUCCESS : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new DistanceQuery(maxDistance, target, checkPlayer);
    }
} 