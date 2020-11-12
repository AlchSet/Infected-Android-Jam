using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{

    public List<Vector3> routePoints = new List<Vector3>();
    public List<Vector3> globalRoutePoints = new List<Vector3>();
    bool p;

    // Start is called before the first frame update
    void Start()
    {
        p = true;
        //int i = 0;
        foreach (Vector3 points in routePoints)
        {
            globalRoutePoints.Add(transform.TransformPoint(points));
            //i++;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnDrawGizmos()
    {
        if (!p)
        {
            foreach (Vector3 point in routePoints)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.TransformPoint(point), 0.5f);
            }
        }
        else
        {
            foreach (Vector3 point in globalRoutePoints)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(point, 0.5f);
            }
        }
        //foreach (Vector3 point in routePoints)
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(point, 0.5f);

        //}
    }

}
