using System.Collections.Generic;

public class Selector : InteriorNode
{
    public Selector(IEnumerable<BehaviorTree> children) : base(children) { }

    public override Result Run()
    {
        while (current_child < children.Count)
        {
            Result res = children[current_child].Run();

            if (res == Result.SUCCESS)
            {
                current_child = 0;
                return Result.SUCCESS;
            }

            if (res == Result.IN_PROGRESS)
            {
                return Result.IN_PROGRESS;
            }

            // On FAILURE, move to next child
            current_child++;
        }

        // All children failed
        current_child = 0;
        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new Selector(CopyChildren());
    }

    public override IEnumerable<BehaviorTree> AllNodes()
    {
        yield return this;

        foreach (BehaviorTree child in children)
        {
            foreach (BehaviorTree node in child.AllNodes())
            {
                yield return node;
            }
        }
    }
}
