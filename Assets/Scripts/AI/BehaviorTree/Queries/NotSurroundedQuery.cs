using UnityEngine;

public class NotSurroundedQuery : BehaviorTree
{
    private int maxAllies;
    private float radius;

    public NotSurroundedQuery(int maxAllies, float radius)
    {
        this.maxAllies = maxAllies;
        this.radius = radius;
    }

    public override Result Run()
    {
        var allies = GameManager.Instance.GetEnemiesInRange(agent.transform.position, radius);
        // subtract self
        int otherAllies = 0;
        foreach (var ally in allies)
        {
            if (ally != agent.gameObject)
                otherAllies++;
        }

        return otherAllies < maxAllies ? Result.SUCCESS : Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new NotSurroundedQuery(maxAllies, radius);
    }
}
