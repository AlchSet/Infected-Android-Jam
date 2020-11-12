using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMouse : MonoBehaviour
{
    public static Vector3 mousePos;
    RaycastHit hit;
    PlayerController player;

    public GameObject chakramBullet;
    bool chakramThrown;


    public LineRenderer tether;

    bool inTether;

    Vector3 pointA;
    Vector3 pointB;

    public float tetherSpeed = 20;

    public float tetherDistance = 10;


    Vector3 tetherPoint;

    //public float tetherPlayerSpeed = 20;
    public float overshoot = 30;

    public bool BOOMERANGMODE;


    public ChakramProjectile.Modes boomode;

    public Animator anim;

    public HitboxManager hmanager;

    bool InAttack;

    public GameObject BoomerangGFX;

    public PostProcessVolume m_Volume;
    //Vignette m_Vignette;
    public ColorGrading m_ColorG;

    public ParticleSystem chakramFX;

    public static ParticleSystem _chakramFX;

    public BoomerangValues boomVal=new BoomerangValues(15,1,5.5f,11.5f,5);


    [System.Serializable]
    public struct BoomerangValues
    {
        public float speed;
        public float time;
        public float angVel1;
        public float angVel2;
        public float yVel;
        

        public BoomerangValues(float sp, float t, float a1,float a2, float ys)
        {
            speed = sp;
            time = t;
            angVel1 = a1;
            angVel2 = a2;
            yVel = ys;
        }


        public void Reset()
        {
            speed = 15;
            time = 1;
            angVel1 = 5.5f;
            angVel2 = 11.5f;
            yVel = 5;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        tether.enabled = false;

        hmanager.OnAttackBegin.AddListener(MeleeAttack);
        hmanager.OnAttackEnd.AddListener(ExitMeleeAttack);

        if (chakramFX)
            _chakramFX = chakramFX;
    }

    // Update is called once per frame
    void Update()
    {

        //Mouse Position in 3D world
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(r.origin, r.direction * 100);
        if (Physics.Raycast(r, out hit, 100))
        {
            mousePos = hit.point;
        }



        Vector3 dir = hit.point - player.transform.position;
        dir.y = 0;


        if (!inTether)
            player.model.rotation = Quaternion.LookRotation(dir.normalized);



        if(Input.GetMouseButtonDown(0)&&!InAttack&&!chakramThrown&&!inTether)
        {
            anim.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonDown(1) && !chakramThrown&&!inTether)
        {
            GameObject g = Instantiate(chakramBullet, (transform.position + Vector3.up) + dir.normalized * 0.2f, Quaternion.identity);
            ChakramProjectile p = g.GetComponent<ChakramProjectile>();

            p.Initiate(player.transform, dir.normalized);
            p.OnComeback.AddListener(ChakramReturned);
            p.MODE=BOOMERANGMODE;
            p.mode = boomode;

            p.Speed = boomVal.speed;
            p.throwTime = boomVal.time;
            p.angleSpeed1 = boomVal.angVel1;
            p.angleSpeed2 = boomVal.angVel2;
            p.yspeed = boomVal.yVel;



            chakramThrown = true;
            BoomerangGFX.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.E) && !chakramThrown && !inTether)
        {

            StartCoroutine(TetherAttack());
        }



        bool slowmo = Input.GetKey(KeyCode.Tab);


        if(slowmo)
        {
            Time.timeScale = 0.25f;
            m_Volume.weight = Mathf.MoveTowards(m_Volume.weight,1,0.1f);
        }
        else
        {
            Time.timeScale = 1f;
            m_Volume.weight = Mathf.MoveTowards(m_Volume.weight,0, 0.1f);
        }




    }

    public void ChakramReturned()
    {
        chakramThrown = false;
        //Debug.Log("RETUrnED");
        BoomerangGFX.SetActive(true);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(mousePos, 0.5f);

        Gizmos.DrawWireSphere(tetherPoint, 0.25f);
    }


    public void MeleeAttack()
    {
        InAttack = true;
    }

    public void ExitMeleeAttack()
    {
        //Debug.Log("END ATTACK");
        InAttack = false;
    }


    IEnumerator TetherAttack()
    {
        tether.enabled = true;
        pointA = transform.position + Vector3.up;
        pointB = mousePos;
        inTether = true;
        player.lockControl = true;
        player.enabled = false;
        Vector3 tPos = pointA;
        float t = 0;
        bool reachPoint = false;
        RaycastHit h;

        //Physics.Linecast(pointA, pointB,out h ,1 << LayerMask.NameToLayer("Default"));

        Debug.DrawLine(pointA, pointB, Color.yellow);
        Ray r = new Ray(pointA, (pointB - pointA).normalized);

        Physics.Raycast(r, out h, 100, 1 << LayerMask.NameToLayer("Default"));

        if (h.collider)
        {
            tetherPoint = h.point;
        }
        else
        {
            Debug.Log("NOTHING");
        }


        while (true)
        {
            t += Time.deltaTime;
            tether.SetPosition(0, pointA);
            tPos = Vector3.MoveTowards(tPos, tetherPoint, tetherSpeed * Time.unscaledDeltaTime);
            tether.SetPosition(1, tPos);
            float distance = Vector3.Distance(pointA, tPos);

            //Debug.Log(distance);

            if (distance > tetherDistance)
            {
                Debug.Log("TOO FAR");
                reachPoint = false;
                break;

            }

            distance = Vector3.Distance(tPos, tetherPoint);

            if (distance <= 0f)
            {
                Debug.Log("REACHED POINT");
                reachPoint = true;
                break;
            }

            //tether.SetPosition(1, Vector3.Lerp(pointA, pointB, t));

            //if (t >= 1)
            //{
            //    break;
            //}

            yield return new WaitForEndOfFrame();
        }

        if (reachPoint)
        {
            while (true)
            {

                Vector3 offset = tetherPoint - transform.position;
                //Get the difference.

                float angle = Vector3.Angle(offset, Vector3.up);

                //Debug.Log(angle);

                //Debug.DrawRay(transform.position + Vector3.up,Vector3.up, Color.green);
                //Debug.DrawRay(transform.position + Vector3.up,Vector3.forward, Color.green);

                if (angle<20)
                {
                    offset = tetherPoint - (transform.position + (Vector3.up * 2));

                    Debug.DrawRay(transform.position + Vector3.up*2, Vector3.up, Color.green);
                    Debug.DrawRay(transform.position + Vector3.up * 2, Vector3.forward, Color.green);
                    //Debug.DrawLine(transform.position + Vector3.up * 2, transform.position + Vector3.up * 4,Color.green);
                }
                else if(angle>20&&angle<45)
                {
                    offset = tetherPoint - (transform.position + (Vector3.up * 1));

                    Debug.DrawRay(transform.position + Vector3.up * 1, Vector3.up, Color.green);
                    Debug.DrawRay(transform.position + Vector3.up * 1, Vector3.forward, Color.green);
                    //Debug.DrawLine(transform.position , transform.position + Vector3.up * 2,Color.green);
                }
                

                //Debug.Log(offset.magnitude);

                //Debug.Log(offset.magnitude + " / " + angle);
                if (offset.magnitude > .71f)
                {
                    //If we're further away than .1 unit, move towards the target.
                    //The minimum allowable tolerance varies with the speed of the object and the framerate. 
                    // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
                    offset = offset.normalized * tetherSpeed;
                    //normalize it and account for movement speed.
                    player.controller.Move(offset * Time.unscaledDeltaTime);
                    //actually move the character.



                }
                else
                {
                    break;
                }

                tether.SetPosition(0, player.transform.position+Vector3.up);
                tether.SetPosition(1, tetherPoint);


                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (true)
            {
                t -= Time.deltaTime;
                tether.SetPosition(0, pointA);
                //tether.SetPosition(1, Vector3.Lerp(pointA, pointB, t));
                tPos = Vector3.MoveTowards(tPos, pointA, tetherSpeed * Time.unscaledDeltaTime);

                tether.SetPosition(1, tPos);

                float distance = Vector3.Distance(pointA, tPos);

                if (distance <= 0)
                {
                    break;
                }

                //if (t <=0)
                //{
                //    break;
                //}

                yield return new WaitForEndOfFrame();
            }
        }

        player.enabled = true;

       
        player.lockControl = false;
        tether.enabled = false;
        if (reachPoint)
            player.controller.Move(r.direction * overshoot * Time.unscaledDeltaTime);
        
        yield return new WaitForSecondsRealtime(0.25f);
        inTether = false;
    }


}
