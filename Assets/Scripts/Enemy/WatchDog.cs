using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchDog : MonoBehaviour
{
    /// <summary>
    /// 看门石像鬼，剧情音效
    /// </summary>

    bool m_IsPlayerInRange;
    public string audioName;
    //public GameEnding gameEnding;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GM.myPlayer)
        {
            AudioController.Instance.SoundPlay(audioName, false, 1f);//播放剧情音效
        }
    }
}
