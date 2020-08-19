using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    /// <summary>
    /// 可交互物体父类
    /// </summary>
    /// 

    protected Animator Anim;
    //public PlayerController player;
    public bool isTrigger = false;
    protected bool isInRange = false;

    public GameObject tips;
    public enum InteractiveObj
    {
        Box,
        LightSwitch,
        DiCi,
        Mucus
    }

    protected virtual void Start()
    {
        Anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform == GM.myPlayer)
        {
            isInRange = true;//判断玩家是否在范围内
            
            if(tips)
            {
                tips.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.transform == GM.myPlayer)
            isInRange = false;

        if (tips)
        {
            tips.SetActive(false);
        }
    }
}
