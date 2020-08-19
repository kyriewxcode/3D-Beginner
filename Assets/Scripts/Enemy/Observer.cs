using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    /// <summary>
    /// 幽灵视线
    /// </summary>
    /// 
    
    bool m_IsPlayerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GM.myPlayer)
        {
            m_IsPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform == GM.myPlayer)
        {
            m_IsPlayerInRange = false;
        }
    }

    private void Update()
    {
        if(m_IsPlayerInRange)
        {
            Vector3 direction = GM.myPlayer.position - transform.position + Vector3.up;//怪物指向玩家的方向
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray,out raycastHit))
            {
                if(raycastHit.collider.transform == GM.myPlayer)
                {
                    EventCenter.Broadcast(EventType.EnemyHunt, raycastHit.transform,transform.parent);//追玩家
                    EventCenter.Broadcast(EventType.TimeLinePlay, "ISeeYou");//当检查者看到玩家会触发剧情音效
                }
            }
        }
    }
}
