using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiCiManager : InteractiveObject
{
    /// <summary>
    /// 地刺管理
    /// </summary>
    /// 

    public float atkRateTime;

    protected override void Update()
    {
        if(GM.playerControl.isDead)
            StopCoroutine("DiCiAtk");
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.transform == GM.myPlayer.transform)
        {
            StartCoroutine("DiCiAtk");
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject == GM.myPlayer.gameObject)
        {
            StopCoroutine("DiCiAtk");
        }
    }
    IEnumerator DiCiAtk()//协程延迟攻击
    {
        while(true)
        {
            yield return new WaitForSeconds(1);//第一次1s就攻击
            EventCenter.Broadcast(EventType.PlayerHurt, 1f);
            EventCenter.Broadcast(EventType.SoundPlay, "DICI", false, 1f);//播放地刺音效
            Anim.SetTrigger("IsAtk");
            yield return new WaitForSeconds(atkRateTime);//后面每atkRateTime攻击一次
        }
    }

}
