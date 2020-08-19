using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLine : MonoBehaviour
{
    /// <summary>
    /// TimeLine管理
    /// </summary>
    /// 

    void Start()
    {
        //如果调用的是该物体的TimeLine 则播放
        EventCenter.AddListener<string>(EventType.TimeLinePlay, (string name) => { 
            if(name==gameObject.name)
            {
                GetComponent<PlayableDirector>().Play();
            }
        });
    }


}
