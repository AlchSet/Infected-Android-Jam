using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent OnHit;

    Transform owner;

    public LayerMask mask;
    // Start is called before the first frame update
    void Awake()
    {
        owner = transform.root;
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;

        Collider[] hit = Physics.OverlapSphere(transform.position, 0.5f,mask);

        foreach (Collider c in hit)
        {
            Damageable d = null;
            if (d = c.GetComponent<Damageable>())
            {
                if(d.transform==owner)
                {
                    Debug.Log("SELF");
                    return;
                }

                if (!d.isHitStun)
                    OnHit.Invoke();
                d.DealDamage(1);

                //Debug.Log("HURT " + d.name);
            }

        }


    }


    private void OnDrawGizmos()
    {
        Color c = Color.red;
        if (enabled)
        {
            c = new Color(1, 0, 0, 0.8f);
        }
        else
        {
            c = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        }

        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
