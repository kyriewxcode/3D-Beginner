using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : HpManager
{
    /// <summary>
    /// 敌人血量控制
    /// </summary>
    /// 

    private Vector3 respwanPos;
    public Transform parentPos;
    public float hurtTime = 0f;
    public Observer view;
    protected override void Start()
    {
        base.Start();
        EventCenter.AddListener<float,Transform>(EventType.EnemyHurt, GetHurt);//添加怪物受伤监听器
    }
    protected override void Update()
    {
        base.Update();
        if (Hp <= 0)
        {
            Respwan();//进入重生程序
        }
    }
    public override void GetHurt(float damage,Transform me)
    {
        if (!isInvincible && transform==me)
        {
            base.GetHurt(damage);
        }
    }
    protected override void Respwan()
    {
        parentPos.GetComponent<AudioSource>().enabled = false;
        GetComponent<Collider>().enabled = false;
        ren.enabled = false;
        view.enabled = false;
        parentPos.position = new Vector3(20, 20, 20);
        if (canRespawn)//如果能重生
        {
            respwanPos = GetComponentInParent<WaypointPatrol>().wayPoints[Random.Range(0, GetComponentInParent<WaypointPatrol>().wayPoints.Length)].position;
            parentPos.position = respwanPos;
            view.enabled = true;
            parentPos.GetComponent<AudioSource>().enabled = true;
            GetComponent<Collider>().enabled = true;
            ren.enabled = true;
            Hp = 1;
            isInvincible = true;
            GetComponentInParent<WaypointPatrol>().navMeshAgent.SetDestination(GetComponentInParent<WaypointPatrol>().wayPoints[0].position);
        }
        base.Respwan();
    }
    private void OnTriggerStay(Collider other)
    {
        hurtTime -= Time.deltaTime;
        if (other.transform == GM.myPlayer && !other.GetComponent<PlayerController>().isDead && hurtTime<=0 && !isInvincible)
        {
            EventCenter.Broadcast(EventType.EnemyAlreadyAtk, transform.parent);
            EventCenter.Broadcast(EventType.PlayerHurt, 1f);
            hurtTime = 1f;
        }
    }
}
