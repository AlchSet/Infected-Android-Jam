using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 testVel;
    public Vector3 pointA;

    public Vector3 pointB;


    private Vector3 pA;
    private Vector3 pB;

    bool p;

    public bool r;

    // Start is called before the first frame update
    void Start()
    {
        pA = transform.TransformPoint(pointA);
        pB = transform.TransformPoint(pointB);

        p = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("PLAt=" + transform.position+" - "+pB);


        if(!r)
        {
            Vector3 d = pB - transform.position;

            float dist = d.magnitude;

            float distBetweenPoints = Vector3.Distance(pA, pB);

            //Debug.Log(dist);


            velocity = d.normalized * (5 * Mathf.Clamp01(dist)) * Time.deltaTime;
            testVel = d.normalized * (5 * Mathf.Clamp01(dist));
            transform.Translate(velocity,Space.World);

            if(dist<0.01f)
            {
                r = true;
            }
        }
        else
        {
            Vector3 d = pA - transform.position;

            float dist = d.magnitude;

            float distBetweenPoints = Vector3.Distance(pA, pB);

            //Debug.Log(dist);


            velocity = d.normalized * (5 * Mathf.Clamp01(dist)) * Time.deltaTime;
            testVel = d.normalized * (5 * Mathf.Clamp01(dist));
            transform.Translate(velocity,Space.World);

            if (dist < 0.01f)
            {
                r = false;
            }
        }
        
        //transform.position = transform.position + velocity;


    }


    private void OnDrawGizmos()
    {
        if(!p)
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(pointA), 0.25f);
            Gizmos.DrawWireSphere(transform.TransformPoint(pointB), 0.25f);
        }
        else
        {
            Gizmos.DrawWireSphere(pA, 0.25f);
            Gizmos.DrawWireSphere(pB, 0.25f);
        }
  
    }


}
