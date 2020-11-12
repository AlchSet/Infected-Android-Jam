using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxManager : MonoBehaviour
{
    public List<Hitbox> hitboxes = new List<Hitbox>();
    public UnityEvent OnAttackBegin;
    public UnityEvent OnAttackEnd;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateHitBoxes(int i)
    {
        hitboxes[i].enabled = true;
    }
    public void DeactivateHitBoxes(int i)
    {
        hitboxes[i].enabled = false;
    }



    public void EvokeOnAttackBegin()
    {
        OnAttackBegin.Invoke();
    }

    public void EvokeOnAttackEnd()
    {
        OnAttackEnd.Invoke();
    }


}
