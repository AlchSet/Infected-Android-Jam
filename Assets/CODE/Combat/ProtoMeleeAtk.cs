using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMeleeAtk : MonoBehaviour
{
    Hitbox hitbox;
    public bool startAtk;
    bool inAtk;
    MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        hitbox = transform.Find("Hitbox").GetComponent<Hitbox>();
        mesh = hitbox.GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(startAtk&&!inAtk)
        {
            StartCoroutine(StartAttack());
        }
    }


    IEnumerator StartAttack()
    {
        inAtk = true;
        Quaternion q1 = Quaternion.Euler(0, 70, 0);
        Quaternion q2 = Quaternion.Euler(0, -70, 0);
        float t = 0;
        mesh.enabled = true;
        hitbox.enabled = true;
        while (true)
        {
            t += Time.deltaTime*4;
            transform.localRotation = Quaternion.Slerp(q1, q2, t);

            if(t>=1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        inAtk = false;
        startAtk = false;
        mesh.enabled = false;
        hitbox.enabled = false;
        yield return null;
    }
}
