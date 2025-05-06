using UnityEngine;

public class HealthQuery : BehaviorTree
{
    private float threshold;
    private bool checkSelf;

    public HealthQuery(float threshold, bool checkSelf = true)
    {
        this.threshold = threshold;
        this.checkSelf = checkSelf;
    }

    public override Result Run()
    {
        if (checkSelf)
        {
            if (agent.hp == null) return Result.FAILURE;
            return agent.hp.hp <= threshold ? Result.SUCCESS : Result.FAILURE;
        }
        else
        {
            // Find closest enemy and check their health
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
            if (closest == null || closest.hp == null) return Result.FAILURE;
            return closest.hp.hp <= threshold ? Result.SUCCESS : Result.FAILURE;
        }
    }

    public override BehaviorTree Copy()
    {
        return new HealthQuery(threshold, checkSelf);
    }
} 