using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : InteractiveObject
{
    /// <summary>
    /// 开关管理
    /// </summary>
    /// 

    bool isOn = true; //灯是否已经开启
    //public Transform tips;

    protected override void Start()
    {
        base.Start();
        //监听 开灯关灯事件
        EventCenter.AddListener(EventType.LightOn, () => { isOn = true; });
        EventCenter.AddListener(EventType.LightOff, () => { isOn = false; });
    }
    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            GM.InputAllowed = false;
            GM.playerControl.GetComponent<Animator>().SetTrigger(isOn ? "SwitchOn" : "SwitchOff");
        }
        if (tips)
        {
            tips.transform.LookAt(Camera.main.transform.position);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.transform == GM.myPlayer)
        {
            EventCenter.AddListener(EventType.SwitchChange, ChangeSwitch);
        }
    }

    void ChangeSwitch() //改变开关动画
    {
        AudioController.Instance.SoundPlay(isOn ? "Switch_LightOff" : "Switch_LightOn", false, 1f);
        Anim.SetTrigger("ChangeSwitch");
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if(other.transform == GM.myPlayer)
        {
            EventCenter.RemoveListener(EventType.SwitchChange, ChangeSwitch);
        }
    }
    void LightChange() // 当快拉下的时候，动画中调用该方法
    {
        if (isOn)
        {
            EventCenter.Broadcast(EventType.LightFlickerOff, transform);
            Debug.Log("关灯");
        }
        else
        {
            EventCenter.Broadcast(EventType.LightFlickerOn, transform);
            Debug.Log("开灯");
        }
        isOn = !isOn;
    }

}
