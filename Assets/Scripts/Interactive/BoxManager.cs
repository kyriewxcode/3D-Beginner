using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxManager : InteractiveObject
{
    /// <summary>
    /// 盒子管理
    /// </summary>

    public Animator getGunAnim;
    bool playerIsFace = false;
    bool playerFacing = false;
    protected override void Start()
    {
        base.Start();
        EventCenter.AddListener(EventType.PlayerIsFace, ()=> { playerIsFace = true; });
    }
    protected override void Update()
    {
        tips.transform.LookAt(Camera.main.transform.position);
        if (Input.GetKeyDown(KeyCode.E) && isInRange && !GM.playerControl.haveGun)
        {
            playerFacing = true;
        }

        if(playerFacing)
        {
            PlayerFacing();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    void PlayerFacing()
    {
        EventCenter.Broadcast(EventType.PlayerFacingObj,transform);
        if(playerIsFace)
        {
            tips.GetComponent<TextMesh>().text = "已打开";
            EventCenter.Broadcast(EventType.BoxOpen);
            Anim.SetTrigger("OpenBox");
            EventCenter.Broadcast(EventType.SoundPlay, "WoodboxOpen", false, 1f);

            getGunAnim.gameObject.SetActive(true);//获得东西UI动画
            getGunAnim.SetTrigger("GetGun");

            GetComponentInChildren<Collider>().isTrigger = true;//打开的宝箱不是障碍物


            GM.playerControl.haveGun = true;
            playerFacing = false;
        }
    }
}
