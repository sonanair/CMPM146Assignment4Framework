using UnityEngine;

public class BehaviorBuilder
{
    public static BehaviorTree MakeTree(EnemyController agent)
    {
        BehaviorTree result = null;

        if (agent.monster == "warlock")
        {
            result = new Selector(new BehaviorTree[] {
                // Baiting: Step toward player to draw fire if few allies around
                new Sequence(new BehaviorTree[] {
                    new PlayerIsFarQuery(5.0f),
                    new NotSurroundedQuery(3, 5.0f),
                    new GoTowards(GameManager.Instance.player.transform, 1.5f, 1.0f)
                }),


                // Standard support behavior
                new Sequence(new BehaviorTree[] {
                    new AbilityReadyQuery("buff"),
                    new StrengthFactorQuery(1.0f),
                    new Buff()
                }),

                new Sequence(new BehaviorTree[] {
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new AbilityReadyQuery("attack"),  // optional, but helpful
                    new Attack()
                })
            });
        }
        else if (agent.monster == "zombie")
        {
            var groupPoint = AIWaypointManager.Instance
                            .GetClosestByType(agent.transform.position, AIWaypoint.Type.FORWARD)
                            .transform;

            result = new Selector(new BehaviorTree[] {
                // 1) Retreat if very low on HP
                new Sequence(new BehaviorTree[] {
                    new LowHPQuery(15.0f),
                    new GoTo(
                    AIWaypointManager.Instance.GetClosestByType(agent.transform.position, AIWaypoint.Type.SAFE)
                                    .transform,
                    1.0f
                    )
                }),

                // 2) Charge the player when enough allies are around
                new Sequence(new BehaviorTree[] {
                    new NearbyEnemiesQuery(3, 6.0f),           // lowered count, larger radius
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),

                // 3) Otherwise, gather at the FORWARD waypoint
                new GoTo(groupPoint, 1.0f)
            });
        }
        else // skeleton
        {
            result = new Selector(new BehaviorTree[] {
                // Buffed → Attack
                new Sequence(new BehaviorTree[] {
                    new StrengthFactorQuery(1.5f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),

                // Enough group → Join in
                new Sequence(new BehaviorTree[] {
                    new NearbyEnemiesQuery(5, 5.0f),
                    new MoveToPlayer(agent.GetAction("attack").range),
                    new Attack()
                }),

                // Else → Flank
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
