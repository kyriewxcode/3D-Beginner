using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EventType
{
    /// <summary>
    /// 事件枚举
    /// </summary>
    /// 

    //Player
    PlayerMove,//玩家移动
    PlayerAim,//玩家瞄准
    PlayerChoseTarget,//玩家选择目标
    PlayerHurt,//玩家受伤
    PlayerDead,//玩家死亡
    PlayerShot,//玩家射击
    PlayerFacingObj,//玩家面向该物体
    PlayerIsFace,//玩家已经面向该物体
    PlayerInit,//玩家初始化

    //Enemy
    EnemyHurt,//敌人受伤
    EnemyRespown,//敌人重生
    EnemyHunt,//敌人猎杀
    EnemyAlreadyAtk,//敌人已经攻击完

    //Object
    LightFlickerOn,//灯芯开
    LightFlickerOff,//灯芯灭
    LightOn,//灯开
    LightOff,//灯关
    LightIsOn,//灯已经开
    LightIsOff,//灯已经关
    SwitchChange,//改变开关
    BoxOpen,//开箱

    //Audio
    SoundPlay, // string 声音名字,float 音量
    SoundStop, // 声音停止
    SoundPause,// 声音暂停
    SoundAllPause,// 所有声音暂停
    BGMPlay,// 播放BGM
    BGMPause,// 暂停 BGM
    BGMSetVolume,// 设置BGM音量

    //TimeLine
    TimeLinePlay,//播放该TimeLine


}
