using UnityEngine;
using System.Collections.Generic;

public class BehaviorTree 
{
    public enum Result { SUCCESS, FAILURE, IN_PROGRESS };

    public EnemyController agent;

    public BehaviorTree() { }

    public virtual Result Run()
    {
        // Log a warning to help debug unimplemented Run methods
        Debug.LogWarning($"[BehaviorTree] Run() called on base class for {this.GetType().Name}. Returning SUCCESS by default.");
        return Result.SUCCESS;
    }

    public void SetAgent(EnemyController agent)
    {
        this.agent = agent;
    }

    public virtual IEnumerable<BehaviorTree> AllNodes()
    {
        yield return this;
    }

    public virtual BehaviorTree Copy()
    {
        Debug.LogWarning($"[BehaviorTree] Copy() not implemented for {this.GetType().Name}");
        return null;
    }
}
