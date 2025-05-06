using System.Collections.Generic;

public class WaitForHPNode : BehaviorTree
{
    private BehaviorTree child;

    public WaitForHPNode(BehaviorTree child)
    {
        this.child = child;
    }

    public override Result Run()
    {
        if (agent == null || agent.hp == null)
            return Result.IN_PROGRESS;
        return child.Run();
    }

    public override BehaviorTree Copy()
    {
        return new WaitForHPNode(child.Copy());
    }

    public override IEnumerable<BehaviorTree> AllNodes()
    {
        yield return this;
        foreach (var node in child.AllNodes())
            yield return node;
    }
} 