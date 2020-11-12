using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MyStateMachine
{
    public class GoToState : StateMachine.State
    {
        NavMeshAgent agent;



        public override void OnStateEnter(StateMachine m)
        {
            if (agent == null)
            {
                agent = m.GetComponent<NavMeshAgent>();
                Debug.Log(name + " ENTER");
            }
            agent.destination = Vector3.zero;
        }

        public override void OnStateExit(StateMachine m)
        {
            Debug.Log(name + " EXIT");
        }

        public override void OnStateUpdate(StateMachine m)
        {
            Debug.Log(name + " UPDATE");
        }


    }
}