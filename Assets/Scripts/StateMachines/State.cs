using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 状态抽象类，具体状态类需要继承此抽象类，以实现该状态具体的内容（3个函数）
public abstract class State
{
    public abstract void Enter();
    // deltatime: 当前帧与上一帧时间的时间间隔
    public abstract void Tick(float deltaTime);
    public abstract void Exit();
}
