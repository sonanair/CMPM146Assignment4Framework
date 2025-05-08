using UnityEngine;

public class InjuredAllyNearbyQuery : BehaviorTree
{
    private float range;

    public InjuredAllyNearbyQuery(float range)
    {
        this.range = range;
    }

    public override Result Run()
    {
        var allies = GameManager.Instance.GetEnemiesInRange(agent.transform.position, range);
        foreach (var ally in allies)
        {
            if (ally == agent.gameObject) continue;

            var ec = ally.GetComponent<EnemyController>();
            if (ec == null || ec.monster == "warlock") continue; // skip self and warlocks

            // check for valid hp and if injured
            if (ec.hp != null && ec.hp.hp < ec.hp.max_hp)
            {
                return Result.SUCCESS;
            }
        }

        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new InjuredAllyNearbyQuery(range);
    }
}
