using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject projectile;

    public int poolSize;


    public List<Bullet> pool = new List<Bullet>();

    public List<Bullet> activeList = new List<Bullet>();

    int index = 0;

    //public List<GameObject> pool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject b = Instantiate(projectile, transform);
        //    b.SetActive(false);
        //    pool.Add(b);
        //}


        for (int i = 0; i < poolSize; i++)
        {
            Bullet b = new Bullet(Instantiate(projectile, transform),transform.position,Vector3.forward,0);
            //b.SetActive(false);
            pool.Add(b);
        }

        StartCoroutine(Cleanup());
    }



    // Update is called once per frame
    void Update()
    {
        foreach(Bullet b in activeList)
        {
            b.UpdateBullet(Time.deltaTime);
        }

    }


    public void InstantiateBullet(Vector3 pos,Vector3 dir,float speed)
    {
        Bullet b = pool[index];
        if(!b.isActive)
        {
            b.Fire(pos, dir, speed);
            activeList.Add(b);
            index = (index + 1) % pool.ToArray().Length;
        }
       
    }


    IEnumerator Cleanup()
    {
        List<Bullet> toRemove = new List<Bullet>();
        while(true)
        {
            foreach(Bullet b in activeList)
            {
                if(!b.isActive)
                {
                    toRemove.Add(b);
                }
            }

            foreach(Bullet b in toRemove)
            {
                activeList.Remove(b);
            }

            toRemove.Clear();


            yield return new WaitForSecondsRealtime(10);
        }
        
    }

    [System.Serializable]
    public class Bullet
    {
        GameObject graphics;
        Vector3 position;
        Vector3 direction;
        float speed;
        float life;

        private bool active;
        public bool isActive
        {
            get
            {
                return active;
            }
            set
            {
                graphics.SetActive(value);
                active = value;

            }
        }
        public Bullet(GameObject g,Vector3 pos,Vector3 dir,float s)
        {
            graphics = g;
            position = pos;
            direction = dir;
            speed = s;
            graphics.transform.position = position;
            isActive = false;
            //graphics.SetActive(false);
        }


        public void Fire(Vector3 pos, Vector3 dir, float s)
        {

            position = pos;
            direction = dir;
            speed = s;
            graphics.transform.position = position;
            graphics.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            isActive = true;
            //graphics.SetActive(true);
        }


        public void UpdateBullet(float delta)
        {
            if(isActive)
            {
                position = position + (direction * speed * delta);
                graphics.transform.position = position;

                Collider[] hits = Physics.OverlapBox(position, new Vector3(0.1f / 2, 0.1f / 2, 0.7f / 2), graphics.transform.rotation);


                life += Time.deltaTime;

                if (hits.Length > 0)
                {

                    if(hits[0].gameObject.layer==LayerMask.NameToLayer("Player"))
                    {
                        hits[0].gameObject.GetComponent<Damageable>().DealDamage(1);
                        Debug.Log("HURT PLAYER");
                    }


                    Debug.Log("Hit stuff");
                    isActive = false;
                }

                if(life>5)
                {
                    life = 0;
                    isActive = false;
                }

                
            }
           
        }


    }
}
