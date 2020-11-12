using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //ENEMIES TO HURT YOU ON CONTACT


    public CharacterController controller;
    Vector3 inputVel;

    public Vector3 velocity;

    Vector3 slidingDirection;

    public Vector3 hitNormal;

    public Vector3 hNorm;

    RaycastHit mOut;

    public bool isGround;

    public float antiBumpFactor=1;


    public float speed=5;

    Vector3 edgeMove;

    public bool charG;

    public Transform model;


    public bool lockControl;

    public Animator anim;

    public Vector3 platformVel;

    Damageable dmg;


    public ParticleSystem splash;

    AudioSource sfx;

    public AudioClip splashSFX;

    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<CharacterController>();
        model = transform.Find("Model");
        dmg = GetComponent<Damageable>();
        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        
        charG = controller.isGrounded;

        if (!lockControl)
        {
            inputVel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

          
        }
        else
        {
            inputVel = Vector3.zero;
        }

        
        
        

        //velocity = inputVel*5;
        velocity.x = inputVel.x * speed;
        velocity.z = inputVel.z * speed;

        if(Mathf.Abs(velocity.x)>0|| Mathf.Abs(velocity.z) > 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        Ray ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray,out mOut,.1f,1<<LayerMask.NameToLayer("Default")))
        {
            hNorm = mOut.normal;
            isGround = true;
        }
        else
        {
            hNorm = Vector3.zero;
            isGround = false;
        }
        //if (Input.GetButtonDown("Jump") && controller.isGrounded)
        //{
        //    Debug.Log("JUMP");
        //    velocity.y = Mathf.Sqrt(2 * 3 * 50);
        //}


       



        if (!controller.isGrounded)
        {
            velocity.y -= 50*Time.unscaledDeltaTime;
        }
        else
        {
           
            velocity.y = -antiBumpFactor;
        }


        if (Input.GetButtonDown("Jump") && controller.isGrounded&&!lockControl)
        {

           // velocity.y = Mathf.Sqrt(2 * 3 * 50);
            velocity.y = Mathf.Sqrt(1 * 3 * 50);
            anim.SetTrigger("Jump");


        }
        //Debug.Log("JUMP " + velocity);



        velocity += edgeMove;
        velocity += platformVel*Time.timeScale;
        edgeMove = Vector3.zero;

        //Debug.Log(((1 + (1 - Time.timeScale)))*Time.deltaTime);
        //controller.Move(velocity * (Time.deltaTime*(1+(1-Time.timeScale))));
        controller.Move(velocity * (Time.unscaledDeltaTime));

        
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.normal);

        hitNormal = hit.normal;
        RaycastHit info;
        platformVel = Vector3.zero;
        if (Physics.Raycast(transform.position,Vector3.down,out info,.3f))
        {

            //Debug.Log(info.collider.name);

            if(info.collider.gameObject.layer==LayerMask.NameToLayer("Platform"))
            {
                //Debug.Log("ON PLATFORM");
                platformVel = info.collider.GetComponent<MovingPlatform>().testVel;
            }


            //Debug.Log("DERP");
        }
        else
        {
            if(controller.isGrounded)
            {
                Debug.Log("ON EDGE");

                edgeMove = transform.position - hit.point;

                edgeMove = edgeMove * 10;
            }
        }

       


        //if (angle > 40f)
        //{
        //    sliding = true;

        //    Vector3 normal = hit.normal;
        //    Vector3 c = Vector3.Cross(Vector3.up, normal);
        //    Vector3 u = Vector3.Cross(c, normal);
        //    slidingDirection = u * 4f;

        //}
        //else
        //{
        //    sliding = false;
        //    slidingDirection = Vector3.zero;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            splash.transform.position = transform.position;
            if (splash)
            {
                splash.Play();
            }

            Debug.Log("DROWN");
            dmg.DealDamage(1);
            sfx.PlayOneShot(splashSFX);


        }
    }


}
