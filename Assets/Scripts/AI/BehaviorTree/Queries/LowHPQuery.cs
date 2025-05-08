public class LowHPQuery : BehaviorTree
{
    private float hpThreshold;

    public LowHPQuery(float hpThreshold)
    {
        this.hpThreshold = hpThreshold;
    }

    public override Result Run()
    {
        if (agent.hp != null && agent.hp.hp < hpThreshold)
        {
            return Result.SUCCESS;
        }
        return Result.FAILURE;
    }

    public override BehaviorTree Copy()
    {
        return new LowHPQuery(hpThreshold);
    }
}
