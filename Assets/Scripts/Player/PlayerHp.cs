using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : HpManager
{
    /// <summary>
    /// 玩家血量管理
    /// </summary>
    /// 

    public Image[] blood = new Image[0];
    public GameObject GameOver;
    public GameObject Win;
    public Transform WinDoor;
    protected override void Start()
    {
        base.Start();
        EventCenter.AddListener<float>(EventType.PlayerHurt, GetHurt);
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void GetHurt(float damage)
    {
        if(!isInvincible)
        {
            base.GetHurt(damage);
            
            isInvincible = true;
            GM.playerControl.Hit1 = true;
            GM.InputAllowed = true;
            blood[(int)Hp].enabled = false;
            if (Hp <= 0)
            {
                GameOver.SetActive(true);
                GM.playerControl.isDead = true;
                Debug.Log("死亡");
                GM.InputAllowed = false;
                EventCenter.Broadcast(EventType.SoundPlay, "Dead", false, 1f); //播放死亡音效
                EventCenter.Broadcast(EventType.TimeLinePlay, "Dead"); //播放死亡TimeLine
            }
            else
            {
                EventCenter.Broadcast(EventType.SoundPlay, "PlayerHurt1", false, 1f); //播放受伤音效
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == WinDoor)
        {
            Win.SetActive(true);
            GM.InputAllowed = false;
            EventCenter.Broadcast(EventType.TimeLinePlay, "Win"); //播放胜利TimeLine
        }
    }

    protected override void Respwan()
    {
        base.Respwan();
    }
}
