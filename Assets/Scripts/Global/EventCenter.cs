using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventCenter
{
    /// <summary>
    /// 消息管理中心
    /// </summary>
    /// 

    //监听字典
    private static Dictionary<EventType, Delegate> eventTable = new Dictionary<EventType, Delegate>();

    //添加监听 检查
    private static void OnListenerAdding(EventType eventType, Delegate callBack)
    {
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable.Add(eventType, null);
        }
        Delegate d = eventTable[eventType];//key为eventType的value
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同的委托，当前事件所对应的委托为{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
        }
    }

    //移除监听 检查
    private static void OnListenerRemoving(EventType eventType, Delegate callBack)
    {
        if (eventTable.ContainsKey(eventType))
        {
            Delegate d = eventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误，事件{0}没有对应委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(String.Format("移除监听错误，尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else //不存在事件码
        {
            throw new Exception(String.Format("移除监听错误，没有事件码{0}", eventType));
        }
    }

//=====================================================================================================================

    //无参 添加监听
    public static void AddListener(EventType eventType, CallBack callback)
    {
        OnListenerAdding(eventType, callback);
        eventTable[eventType] = (CallBack)eventTable[eventType] + callback;//要添加的委托和已经存在委托一致了,关联一下
    }
    //一个参数 添加监听
    public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerAdding(eventType, callBack);
        eventTable[eventType] = (CallBack<T>)eventTable[eventType] + callBack;
    }
    //两个参数 添加监听
    public static void AddListener<T,X>(EventType eventType, CallBack<T,X> callBack)
    {
        OnListenerAdding(eventType, callBack);
        eventTable[eventType] = (CallBack<T,X>)eventTable[eventType] + callBack;
    }
    //三个参数 添加监听
    public static void AddListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
    {
        OnListenerAdding(eventType, callBack);
        eventTable[eventType] = (CallBack<T, X, Y>)eventTable[eventType] + callBack;
    }

    //=====================================================================================================================

    //无参 移除监听
    public static void RemoveListener(EventType eventType, CallBack callBack)
    {
        OnListenerRemoving(eventType, callBack);
        eventTable[eventType] = (CallBack)eventTable[eventType] - callBack;

        if (eventTable[eventType] == null) //如果为空了，移除事件码
        {
            eventTable.Remove(eventType);
        }
    }
    //一个参数 移除监听
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        eventTable[eventType] = (CallBack<T>)eventTable[eventType] - callBack;

        if (eventTable[eventType] == null) //如果为空了，移除事件码
        {
            eventTable.Remove(eventType);
        }
    }
    //两个参数 移除监听
    public static void RemoveListener<T,X>(EventType eventType, CallBack<T, X> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        eventTable[eventType] = (CallBack<T, X>)eventTable[eventType] - callBack;

        if (eventTable[eventType] == null) //如果为空了，移除事件码
        {
            eventTable.Remove(eventType);
        }
    }
    //三个参数 移除监听
    public static void RemoveListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        eventTable[eventType] = (CallBack<T, X, Y>)eventTable[eventType] - callBack;

        if (eventTable[eventType] == null) //如果为空了，移除事件码
        {
            eventTable.Remove(eventType);
        }
    }

    //=====================================================================================================================

    //无参 广播监听
    public static void Broadcast(EventType eventType)
    {
        //把事件码对应的委托取出来，调用一下委托
        if (eventTable.TryGetValue(eventType, out Delegate d)) //尝试获取该键的值
        {
            //当参数不匹配时会强转失败
            if (d is CallBack callBack)
            {
                callBack();
            }
            else
            {
                throw new Exception(String.Format("广播事件错误，事件{0}对应委托有不同的类型，", eventType));
            }
        }
    }
    //一个参数参 广播监听
    public static void Broadcast<T>(EventType eventType, T arg)
    {
        if (eventTable.TryGetValue(eventType, out Delegate d)) //尝试获取该键的值
        {
            if (d is CallBack<T> callBack)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(String.Format("广播事件错误，事件{0}对应委托有不同的类型，", eventType));
            }
        }
    }
    //两个参数参 广播监听
    public static void Broadcast<T,X>(EventType eventType, T arg, X arg1)
    {
        if (eventTable.TryGetValue(eventType, out Delegate d)) //尝试获取该键的值
        {
            if (d is CallBack<T,X> callBack)
            {
                callBack(arg, arg1);
            }
            else
            {
                throw new Exception(String.Format("广播事件错误，事件{0}对应委托有不同的类型，", eventType));
            }
        }
    }
    //三个参数参 广播监听
    public static void Broadcast<T, X, Y>(EventType eventType, T arg, X arg1,Y arg2)
    {
        if (eventTable.TryGetValue(eventType, out Delegate d)) //尝试获取该键的值
        {
            if (d is CallBack<T, X, Y> callBack)
            {
                callBack(arg, arg1, arg2);
            }
            else
            {
                throw new Exception(String.Format("广播事件错误，事件{0}对应委托有不同的类型，", eventType));
            }
        }
    }
}
