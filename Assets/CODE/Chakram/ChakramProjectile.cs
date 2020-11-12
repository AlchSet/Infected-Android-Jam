using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChakramProjectile : MonoBehaviour
{
    public enum Modes { SIMPLE, ANGLEBASED }

    public Modes mode = Modes.SIMPLE;

    float elapsedTime;
    Vector3 dir;
    Transform player;
    bool comeback;
    Rigidbody body;
    Vector3 dest;
    Vector3 v;
    public UnityEvent OnComeback;
    bool isStopped;
    public bool MODE;
    float t;



    Vector3 vtemp;


    public float throwTime = 1f;


    public float MoveAngle;

    public float Speed = 15;

    public float angleSpeed1 = 5.5f;
    public float angleSpeed2 = 11.5f;
    public float yspeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        switch (mode)
        {
            case Modes.SIMPLE:

                if (!comeback)
                {
                    //body.MovePosition(transform.position + ((dir * 3) * Time.deltaTime));
                    //elapsedTime += Time.deltaTime;

                    //if(elapsedTime>=1.5f)
                    //{
                    //    comeback = true;
                    //}
                    if (!MODE)
                    {
                        body.MovePosition(Vector3.MoveTowards(transform.position, dest, Time.deltaTime * 15));
                        //body.MovePosition(Vector3.SmoothDamp(transform.position, dest,ref v ,0.4f));

                        float d = Vector3.Distance(transform.position, dest);
                        //Debug.Log(d);
                        if (d <= 0.1f)
                        {
                            comeback = true;
                        }
                    }
                    else
                    {
                        t += Time.deltaTime;
                        Vector3 Dest = PlayerMouse.mousePos;
                        Dest.y = player.transform.position.y + 1;




                        //Vector3 stutter = new Vector3(Mathf.Sin(Time.time*2), 0, Mathf.Cos(Time.time*2));

                        //Dest = Dest + stutter;
                        body.MovePosition(Vector3.MoveTowards(transform.position, Dest, Time.deltaTime * 15));
                        //body.MovePosition(Vector3.SmoothDamp(transform.position, Dest,ref vtemp ,Time.deltaTime * 15));

                        //Vector3 moveD =  Dest-transform.position;

                        //moveD = moveD.normalized * 15*Time.deltaTime;

                        //body.MovePosition(transform.position + moveD);

                        if (t >= throwTime)
                        {
                            comeback = true;
                        }
                    }


                }
                else
                {
                    body.MovePosition(Vector3.MoveTowards(transform.position, player.position + Vector3.up, Time.deltaTime * 15));
                    //body.MovePosition(Vector3.SmoothDamp(transform.position, player.position, ref v, 0.3f));


                    float d = Vector3.Distance(transform.position, player.position + Vector3.up);

                    if (d <= .16f)
                    {
                        OnComeback.Invoke();
                        Destroy(gameObject);
                    }

                }
                break;

            case Modes.ANGLEBASED:


                if (!comeback)
                {

                    Vector3 d = PlayerMouse.mousePos - transform.position;
                    d.Normalize();
                    d.y = 0;
                    float angle = Mathf.Atan2(d.z, d.x) * Mathf.Rad2Deg;

                    MoveAngle = Mathf.MoveTowardsAngle(MoveAngle, angle, angleSpeed1);


                    Vector3 vel = new Vector3(Mathf.Cos(MoveAngle * Mathf.Deg2Rad), 0, Mathf.Sin(MoveAngle * Mathf.Deg2Rad));




                    Debug.DrawRay(transform.position, vel.normalized * 10, Color.yellow);

                    Vector3 finalVel = transform.position + (vel * Speed * Time.deltaTime);

                    //finalVel.y = Mathf.MoveTowards(transform.position.y, player.position.y + 1, Time.deltaTime * 15);

                    //body.MovePosition(transform.position + (vel * Speed * Time.deltaTime));
                    body.MovePosition(finalVel);

                    t += Time.deltaTime;

                    if (t >= throwTime)
                    {
                        comeback = true;
                    }

                }
                else
                {
                    Vector3 d = player.position - transform.position;
                    d.Normalize();
                    d.y = 0;
                    float angle = Mathf.Atan2(d.z, d.x) * Mathf.Rad2Deg;

                    MoveAngle = Mathf.MoveTowardsAngle(MoveAngle, angle, angleSpeed2);


                    Vector3 vel = new Vector3(Mathf.Cos(MoveAngle * Mathf.Deg2Rad), 0, Mathf.Sin(MoveAngle * Mathf.Deg2Rad));




                    Debug.DrawRay(transform.position, vel.normalized * 10, Color.yellow);

                    Vector3 finalVel = transform.position + (vel * Speed * Time.deltaTime);

                    finalVel.y = Mathf.MoveTowards(transform.position.y, player.position.y + 1, Time.deltaTime * yspeed);

                    //body.MovePosition(transform.position + (vel * Speed * Time.deltaTime));
                    body.MovePosition(finalVel);


                    float dist = Vector3.Distance(transform.position, player.position + Vector3.up);

                    if (dist <= .75f)
                    {
                        OnComeback.Invoke();
                        Destroy(gameObject);
                    }


                }



                break;
        }



    }


    public void Initiate(Transform player, Vector3 direction)
    {
        this.player = player;
        dir = direction;

        dest = (player.position + Vector3.up) + dir * 10;

        Vector3 d = PlayerMouse.mousePos - player.position;
        d.Normalize();
        d.y = 0;
        MoveAngle = Mathf.Atan2(d.z, d.x) * Mathf.Rad2Deg;


    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            body.velocity = Vector3.zero;
            body.isKinematic = true;
            isStopped = true;
            this.enabled = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Debug.Log("HIT ENEMY");

            float d = Vector3.Distance(other.transform.position, transform.position);
            float rsum = 0.75f + 0.5f;

            if(PlayerMouse._chakramFX)
            {
                PlayerMouse._chakramFX.transform.position = transform.position;
                PlayerMouse._chakramFX.Emit(10);
            }

            Debug.Log(rsum - d);
            if(rsum>d)
            {

            }
            else if(rsum<d)
            {
                //Doesnt Intersect
            }

            other.GetComponent<Damageable>().DealDamage(1);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && isStopped && other.transform.root.CompareTag("Player"))
        {
            OnComeback.Invoke();
            Destroy(gameObject);
        }
    }


}
