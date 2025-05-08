using UnityEngine;

public class PlayerIsFarQuery : BehaviorTree
{
    private float distance;

    public PlayerIsFarQuery(float distance)
    {
        this.distance = distance;
    }

    public override Result Run()
    {
        if (Vector3.Distance(agent.transform.position, GameManager.Instance.player.transform.position) > distance)
        {
            return Result.SUCCESS;
        }
        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new PlayerIsFarQuery(distance);
    }
}
