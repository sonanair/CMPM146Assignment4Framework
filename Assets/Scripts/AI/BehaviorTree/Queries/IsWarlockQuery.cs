using UnityEngine;

public class IsWarlockQuery : BehaviorTree
{
    private bool checkSelf;

    public IsWarlockQuery(bool checkSelf = true)
    {
        this.checkSelf = checkSelf;
    }

    public override Result Run()
    {
        if (checkSelf)
        {
            if (agent == null) return Result.FAILURE;
            return agent.monster == "warlock" ? Result.SUCCESS : Result.FAILURE;
        }
        else
        {
            // Find closest enemy and check if they're a warlock
            EnemyController closest = null;
            float minDist = float.MaxValue;
            foreach (var enemy in GameObject.FindObjectsOfType<EnemyController>())
            {
                if (enemy == agent) continue;
                float dist = Vector3.Distance(agent.transform.position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemy;
                }
            }
            if (closest == null) return Result.FAILURE;
            return closest.monster == "warlock" ? Result.SUCCESS : Result.FAILURE;
        }
    }

    public override BehaviorTree Copy()
    {
        return new IsWarlockQuery(checkSelf);
    }
} 