using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpManager : MonoBehaviour
{
    /// <summary>
    /// 血量管理父类
    /// </summary>
    /// 

    protected enum Type
    {
        enemy,player
    }

    public float Hp;
    private Animator Anim;
    public Renderer ren;

    public bool isInvincible = false;
    public float invinvibleTime;
    public float invincibleSpentTime;

    public bool canMove = true;
    public bool canRespawn = true;
    public float invincibleSpeed;

    protected virtual void Start()
    {
        Anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (isInvincible)//无敌效果
        {
            invincibleSpentTime += Time.deltaTime;

            if (invincibleSpentTime < invinvibleTime)
            {
                float remainder = invincibleSpentTime % invincibleSpeed; //闪烁计算
                ren.enabled = remainder > invincibleSpeed/2;
            }
            else
            {
                ren.enabled = true;
                isInvincible = false;
            }
        }
    }

    public virtual void GetHurt(float damage,Transform me)
    {

    }
    public virtual void GetHurt(float damage)
    {
        Hp -= damage;
        invincibleSpentTime = 0f;
        if (Hp <= 0)
        {
            canMove = false;
            if(canRespawn)
                Invoke("ReleaseMove", 3f);
        }
    }

    protected virtual void Respwan()
    {
        if(canRespawn)
        {
            isInvincible = true;
        }
    }

    void ReleaseMove()
    {
        canMove = true;
        CancelInvoke("ReleaseMove");
    }


}
