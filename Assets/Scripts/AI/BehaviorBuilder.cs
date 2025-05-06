using UnityEngine;

public class BehaviorBuilder
{
    private static float SafeAttackRange(EnemyController agent, float fallback = 1.5f)
    {
        var action = agent.GetAction("attack");
        if (action != null)
            return action.range;
        return fallback;
    }

    private static Transform SafePlayerTransform()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            return playerObj.transform;
        return null;
    }

    public static BehaviorTree MakeTree(EnemyController agent)
    {
        BehaviorTree result = null;
        var playerTransform = SafePlayerTransform();
        if (agent.monster == "warlock")
        {
            result = new Sequence(new BehaviorTree[] {
                new MoveToPlayer(SafeAttackRange(agent)),
                new Attack(),
                new PermaBuff(),
                new Heal(),
                new Buff()
            });
        }
        else if (agent.monster == "zombie")
        {
            result = new Selector(new BehaviorTree[] {
                new Sequence(new BehaviorTree[] {
                    new NearbyEnemiesQuery(2, 2.5f),
                    new DistanceQuery(1.5f, null, true),
                    new Attack()
                }),
                new Sequence(new BehaviorTree[] {
                    new NotNode(new NearbyEnemiesQuery(2, 2.5f)),
                    new DistanceQuery(2f, null, true),
                    playerTransform != null ?
                        (BehaviorTree)new GoTowards(playerTransform, 2f, 0.5f) :
                        (BehaviorTree)new MoveToPlayer(SafeAttackRange(agent))
                }),
                new Sequence(new BehaviorTree[] {
                    new MoveToPlayer(SafeAttackRange(agent)),
                    new Attack()
                }),
                new MoveToPlayer(SafeAttackRange(agent))
            });
        }
        else
        {
            result = new Sequence(new BehaviorTree[] {
                new MoveToPlayer(SafeAttackRange(agent)),
                new Attack()
            });
        }

        foreach (var n in result.AllNodes())
        {
            n.SetAgent(agent);
        }
        return result;
    }
}
