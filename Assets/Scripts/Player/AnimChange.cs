using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimChange : MonoBehaviour
{
    /// <summary>
    /// 角色动画控制
    /// </summary>
    Animator Anim;
    WeaponControl weaponControl;

    void Start()
    {
        Anim = GetComponent<Animator>();
        weaponControl = GetComponent<WeaponControl>();
        EventCenter.AddListener(EventType.BoxOpen, () => { Anim.SetTrigger("BoxOpen"); });
    }

    void Update()
    {
        Anim.SetBool("IsWalking", GM.playerControl.isWalking);
        Anim.SetBool("HaveGun", GM.playerControl.haveGun);
        Anim.SetBool("IsFire", weaponControl.isFire);
        if (GM.playerControl.Hit1)
        {
            GM.playerControl.Hit1 = false;
            Anim.SetTrigger("Hit01");
        }
        if (GM.playerControl.Hit2)
        {
            GM.playerControl.Hit2 = false;
            Anim.SetTrigger("Hit02");
        }

        if (GM.playerControl.boxOpen)
        {
            Anim.SetTrigger("BoxOpen");
            GM.playerControl.boxOpen = false;
        }
    }
    void SwitchChange()
    {
        EventCenter.Broadcast(EventType.SwitchChange);
    }

}
