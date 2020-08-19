using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 枚举多个参数的事件委托
/// </summary>
/// 

public delegate void CallBack();
public delegate void CallBack<T>(T arg);
public delegate void CallBack<T, X>(T arg, X arg1);
public delegate void CallBack<T, X, Y>(T arg, X arg1, Y arg2);
public delegate void CallBack<T, X, Y, Z>(T arg, X arg1, Y arg2, Z arg3);
public delegate void CallBack<T, X, Y, Z, W>(T arg, X arg1, Y arg2, Z arg3, W arg4);
