using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MucusManager : InteractiveObject
{
    /// <summary>
    /// 减速陷阱
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.gameObject == GM.playerControl.gameObject)
        {
            GM.playerControl.walkSpeed /= 2;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject == GM.playerControl.gameObject)
        {
            GM.playerControl.walkSpeed *= 2;
        }
    }
}
