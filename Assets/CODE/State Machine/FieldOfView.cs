using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public bool detectPlayer;

    public Transform target;


    public float minDetectDistance = 20;

    [Range(1,180)]
    public float fieldAngle = 45;

    public Vector3 eyes;

    public MyStateMachine.StateMachine ai;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 d = target.position - (transform.position);
        d.y = 0;
        Debug.DrawRay(transform.position, d.normalized * 100,Color.yellow);
        if(d.magnitude<= minDetectDistance)
        {

            float angle = Vector3.Angle(d.normalized,transform.forward);
            //Debug.Log(angle+" /"+fieldAngle+" /"+(fieldAngle*2));
            if (angle<=fieldAngle)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position + eyes, target.position,out hit))
                {
                    //Debug.Log(hit.collider.name);

                    if(hit.collider.tag=="Player")
                    {
                        detectPlayer = true;
                    }
                    else
                    {
                        detectPlayer = false;
                    }
                    
                }
                else
                {
                    detectPlayer = false;
                }
                
            }
            else
            {
                detectPlayer = false;
            }

            

        }
        else
        {
            detectPlayer = false;
        }


        ai.parameters[0].boolType = detectPlayer;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position+eyes, minDetectDistance);

        Vector3 t = Quaternion.AngleAxis(fieldAngle, Vector3.up)*transform.forward;
        Vector3 t2 = Quaternion.AngleAxis(-fieldAngle, Vector3.up)*transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position+eyes, t*minDetectDistance);
        Gizmos.DrawRay(transform.position+eyes, t2*minDetectDistance);
        
        
    }




}
