using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TargetDetector", story: "Update [targetDetector] and set [target]", category: "Action", id: "b3c92ffc9d33ce1e40e47865e01ec660")]
public partial class TargetDetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<TargetDetector> TargetDetector;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        Target.Value = TargetDetector.Value.UpdateDetector();
        return Target.Value ? Status.Success : Status.Failure; 
    }
}

