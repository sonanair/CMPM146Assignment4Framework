using System.Collections.Generic;

public class Selector : InteriorNode
{
    private int current_child = 0;

    public override Result Run()
    {
        if (current_child >= children.Count)
        {
            current_child = 0;
            return Result.FAILURE;
        }
        Result res = children[current_child].Run();
        if (res == Result.SUCCESS)
        {
            current_child = 0;
            return Result.SUCCESS;
        }
        if (res == Result.FAILURE)
        {
            current_child++;
        }
        return Result.IN_PROGRESS;
    }

    public Selector(IEnumerable<BehaviorTree> children) : base(children)
    {
    }

    public override BehaviorTree Copy()
    {
        return new Selector(CopyChildren());
    }
}
