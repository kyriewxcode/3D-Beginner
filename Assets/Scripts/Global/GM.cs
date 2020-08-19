using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    /// <summary>
    /// 单例GameManager
    /// </summary>
    /// 

    private static GM instance;
    public static GM Instance
    {
        get
        {
            if(instance = null)
                instance = new GM();
            return instance;
        }
        set { }
    }

    public static bool InputAllowed = false; //用户是否能输入

    //玩家角色
    public static Transform myPlayer;
    public static PlayerController playerControl;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        EventCenter.AddListener(EventType.PlayerInit, PlayerInit);
        
    }

    void PlayerInit()
    {
        Debug.Log("初始化");
        EventCenter.Broadcast(EventType.BGMPlay, "BGM", 1f);//播放背景音乐
        InputAllowed = true;
    }
}
