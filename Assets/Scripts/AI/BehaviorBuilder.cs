using UnityEngine;

public class BehaviorBuilder
{
    public static BehaviorTree MakeTree(EnemyController agent)
    {
        BehaviorTree result = null;

        if (agent.monster == "warlock")
        {
            result = new Selector(new BehaviorTree[] {
                // Heal lowest health ally if possible
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("heal"),
                    new NearbyEnemiesQuery(1, 5.0f), // count = 1, range = 5 units
                    new Heal()
                }),

                // Apply permanent buff to strong ally
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("permabuff"),
                    new StrengthFactorQuery(1.5f),
                    new PermaBuff()
                }),

                // Apply temporary buff if stronger enemies are around
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("buff"),
                    new StrengthFactorQuery(1.0f),
                    new Buff()
                }),

                // Support fallback: stay at support waypoint (index 1)
                new GoTo(AIWaypointManager.Instance.Get(1).transform, 1.0f)
            });
        }
        else if (agent.monster == "zombie")
        {
            result = new Selector(new BehaviorTree[] {
                // If group size is big enough, move to player and attack
                new Sequence(new BehaviorTree[] {
                    new NearbyEnemiesQuery(4, 5.0f), // count = 4, radius = 5 units
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),

                // Else group at a waypoint (index 0)
                new GoTo(AIWaypointManager.Instance.Get(0).transform, 1.0f)
            });
        }
        else // skeleton
        {
            result = new Selector(new BehaviorTree[] {
                // If buffed, attack player
                new Sequence(new BehaviorTree[] {
                    new StrengthFactorQuery(1.5f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),

                // Else move to flank position (index 2)
                new GoTo(AIWaypointManager.Instance.Get(2).transform, 1.0f)
            });
        }

        // do not change/remove: each node should be given a reference to the agent
        foreach (var n in result.AllNodes())
        {
            n.SetAgent(agent);
        }

        return result;
    }
}
