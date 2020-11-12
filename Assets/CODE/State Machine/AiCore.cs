using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCore : MonoBehaviour
{
    public List<Route> routeList = new List<Route>();
    public Gun gun;
    public ProtoMeleeAtk melee;
    public NavMeshAgent agent;

    public bool hasMeleeATK;
    public bool hasRangedATK;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (melee)
            hasMeleeATK = true;

        if (gun)
            hasRangedATK = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
