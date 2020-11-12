using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyStateMachine
{
    public class ChaseState : StateMachine.State
    {

        Transform player;
        AiCore ai;
        FieldOfView sight;
        Vector3 lastseenPos;
        Vector3 lastseenDir;


        float lastspeed;
        float lastStopDist;
        float stopDist = 2;
        float speed=6;

        float t=5;


        SearchStatus searchState;

        delegate void Action();

        Action action;

        Coroutine waitRoutine;


        bool fire;
        float fireElapsed;


        struct SearchStatus
        {
            public bool search;
            public bool firstCheck;
            public Quaternion searchDir1;
            public Quaternion searchDir2;


            public void CheckedFirstDir()
            {
                firstCheck = true;
            }

            public void Reset()
            {
                search = false;
                firstCheck = false;
            }
        }
        

        public override void OnStateEnter(StateMachine m)
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;

                //Debug.Log(player.ToString());
            }

            if (ai == null)
            {
                ai = m.GetComponent<AiCore>();
                lastspeed = ai.agent.speed;
                lastStopDist = ai.agent.stoppingDistance;
                ai.agent.speed = speed;

            }

            if(sight==null)
            {
                sight = m.GetComponent<FieldOfView>();
            }

            searchState.Reset();
        }

        public override void OnStateExit(StateMachine m)
        {
            ai.agent.speed = lastspeed;
            ai.agent.stoppingDistance = lastStopDist;
            ai.agent.updateRotation = true;
            //throw new System.NotImplementedException();
        }

        public override void OnStateUpdate(StateMachine m)
        {
            //Debug.Log("State=")

            if(sight.detectPlayer)
            {
                ai.agent.speed = speed;
                ai.agent.updateRotation = false;
                t = 0;
                ai.agent.destination = player.position;
                ai.agent.stoppingDistance = stopDist;
                lastseenDir = player.position - lastseenPos;


                
                lastseenDir.Normalize();

                lastseenPos = player.position;
                //ai.agent.updateRotation = true;
                searchState.Reset();


                Vector3 d =player.position - ai.transform.position;

                Vector3 dr = d.normalized;
                dr.y = 0;
                //float d = (ai.transform.position - player.position).sqrMagnitude;
                ai.transform.rotation = Quaternion.RotateTowards(ai.transform.rotation, Quaternion.LookRotation(dr.normalized), 8.2f);

                if(ai.hasMeleeATK&&ai.hasRangedATK)
                {
                    if (d.sqrMagnitude > 3f)
                    {
                        //if (!ai.hasRangedATK)
                        //    return;


                        if (!fire)
                        {
                            ai.gun.Fire(d.normalized, 30);
                            fire = true;
                        }
                        else
                        {
                            fireElapsed += Time.deltaTime;
                            if (fireElapsed >= 2)
                            {
                                fire = false;
                                fireElapsed = 0;
                            }
                        }
                    }
                    else
                    {
                        //if (!ai.hasMeleeATK)
                        //    return;


                        if (!fire)
                        {
                            ai.melee.startAtk = true;
                            fire = true;
                        }
                        else
                        {
                            fireElapsed += Time.deltaTime;
                            if (fireElapsed >= .5f)
                            {
                                fire = false;
                                fireElapsed = 0;
                            }
                        }
                    }
                }
                else if(!ai.hasMeleeATK&&ai.hasRangedATK)
                {
                    if (!fire)
                    {
                        ai.gun.Fire(d.normalized, 30);
                        fire = true;
                    }
                    else
                    {
                        fireElapsed += Time.deltaTime;
                        if (fireElapsed >= 2)
                        {
                            fire = false;
                            fireElapsed = 0;
                        }
                    }
                }
                else if(ai.hasRangedATK&&!ai.hasMeleeATK)
                {
                    if (!fire)
                    {
                        ai.melee.startAtk = true;
                        fire = true;
                    }
                    else
                    {
                        fireElapsed += Time.deltaTime;
                        if (fireElapsed >= .5f)
                        {
                            fire = false;
                            fireElapsed = 0;
                        }
                    }
                }
                else
                {

                }



                


            }
            else
            {
                ai.agent.destination = lastseenPos;
                ai.agent.stoppingDistance = 0;
                
                //float d = Vector3.Distance(m.transform.position, lastseenPos);

                //Debug.Log(ai.agent.remainingDistance);
                if(ai.agent.remainingDistance<=0)
                {

                    if(!searchState.search)
                    {
                        InitSearch();
                        searchState.search = true;
                        //action = searchState.CheckedFirstDir;
                    }
                    else
                    {

                        if(!searchState.firstCheck)
                        {
                            ai.transform.rotation = Quaternion.RotateTowards(ai.transform.rotation, searchState.searchDir1, 2.2f);

                            float angle = Quaternion.Angle(ai.transform.rotation, searchState.searchDir1);

                            if (angle <= 0)
                            {
                                if (waitRoutine == null)
                                {
                                    waitRoutine = m.StartCoroutine(WaitForaWhile());
                                }
                                //searchState.firstCheck = true;
                            } 

                            //Debug.Log("Search angle=" + angle);

                        }
                        else
                        {
                            ai.transform.rotation = Quaternion.RotateTowards(ai.transform.rotation, searchState.searchDir2, 2.2f);

                            float angle = Quaternion.Angle(ai.transform.rotation, searchState.searchDir2);

                            if (angle <= 0)
                            {
                                if (waitRoutine == null)
                                {
                                    waitRoutine = m.StartCoroutine(WaitForaWhile());
                                }
                                //searchState.firstCheck = true;
                            }

                            //Debug.Log("Search angle=" + angle);



                        }
                        //t += Time.deltaTime*0.1f;
                        //ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, searchState.searchDir1, t);




                    }
                    
                }
                else
                {
                    ai.agent.updateRotation = true;
                    //ai.transform.rotation = Quaternion.RotateTowards(ai.transform.rotation, Quaternion.LookRotation(lastseenDir), 1.2f);
                    //ai.transform.rotation = Quaternion.LookRotation(lastseenDir,Vector3.up);
                }
            }
            
        }

        public void InitSearch()
        {
            searchState.searchDir1 = Quaternion.AngleAxis(90, Vector3.up) * m.transform.rotation;
            searchState.searchDir2 = Quaternion.AngleAxis(-90, Vector3.up) * m.transform.rotation;
        }


        public void NextState()
        {
            m.parameters[1].TriggerType = true;
        }

        IEnumerator WaitForaWhile()
        {
            yield return new WaitForSeconds(3);


            if(!searchState.firstCheck)
            {
                searchState.firstCheck = true;
            }
            else
            {
                NextState();
            }
            waitRoutine = null;
            //action();

        }



    }
}