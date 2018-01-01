using System;
using UnityEngine;

namespace DongerAssetPack.IceEngine
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (MovementController))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public MovementController character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<MovementController>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                var abilityArgs = new AbilityArgs();
                abilityArgs.Move = agent.desiredVelocity;
                abilityArgs.Jump = false;
                abilityArgs.Crouch = false;
                character.Move(abilityArgs);
            }
            else
            {
                var abilityArgs = new AbilityArgs();
                abilityArgs.Move = Vector3.zero;
                abilityArgs.Jump = false;
                abilityArgs.Crouch = false;
                character.Move(abilityArgs);
            }
                
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
