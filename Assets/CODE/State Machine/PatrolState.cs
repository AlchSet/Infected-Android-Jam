using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyStateMachine
{
    public class PatrolState : StateMachine.State
    {

        AiCore ai;
        public int selectedRoute;
        Route r;

        int waypointIndex;

        public override void OnStateEnter(StateMachine m)
        {
            if(ai==null)
            {
                ai = m.GetComponent<AiCore>();
                r = ai.routeList[selectedRoute];

            }
            ai.agent.destination = r.routePoints[waypointIndex];
        }

        public override void OnStateExit(StateMachine m)
        {
            Debug.Log("EXIT " + name);
            //throw new System.NotImplementedException();
        }

        public override void OnStateUpdate(StateMachine m)
        {

            //Debug.Log(waypointIndex);
            ai.agent.destination = r.globalRoutePoints[waypointIndex];
            
            if(ai.agent.remainingDistance<=0)
            {
                waypointIndex = (waypointIndex + 1) % r.routePoints.ToArray().Length;
                ai.agent.destination = r.globalRoutePoints[waypointIndex];
            }
        }
    }
}