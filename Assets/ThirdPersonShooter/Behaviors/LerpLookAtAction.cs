using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace ThirdPersonShooter.Behaviors
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "LerpLookAt", story: "[self] look at [target] at [speed] speed", category: "Action", id: "5bb9ee5d36da6a60f7b21784c02f8c33")]
    public partial class LerpLookAtAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<float> Speed;
    
        [Tooltip("True: the node process the LookAt every update with status Running." +
                 "\nFalse: the node process the LookAt only once.")]
        [SerializeReference] public BlackboardVariable<bool> Continuous = new BlackboardVariable<bool>(false);
        [SerializeReference] public BlackboardVariable<bool> LimitToYAxis = new BlackboardVariable<bool>(false);


        protected override Status OnStart()
        {
            if (Self.Value == null || Target.Value == null)
            {
                LogFailure($"Missing Transform or Target.");
                return Status.Failure;
            }

            ProcessLookAt();
            return Continuous.Value ? Status.Running : Status.Success;
        }

        protected override Status OnUpdate()
        {
            if (Continuous.Value)
            {
                ProcessLookAt();
                return Status.Running;
            }
            return Status.Success;
        }

        void ProcessLookAt()
        {
            Vector3 targetPosition = Target.Value.transform.position;

            if (LimitToYAxis.Value)
            {
                targetPosition.y = Self.Value.transform.position.y;
            }
        
            Quaternion lookAtDirection = Quaternion.LookRotation(targetPosition - Self.Value.transform.position);
            Self.Value.transform.rotation = Quaternion.Lerp(Self.Value.transform.rotation, lookAtDirection, Speed * Time.deltaTime);
        }
    }
}

