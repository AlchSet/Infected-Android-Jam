using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public BulletManager bullet;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    bullet.InstantiateBullet(transform.position, transform.forward, 15);
        //}
    }

    public void Fire(Vector3 dir,float s)
    {
        bullet.InstantiateBullet(transform.position, dir, s);
    }

}
