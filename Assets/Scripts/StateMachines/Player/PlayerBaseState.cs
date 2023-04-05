using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 角色基本状态：状态需要存状态机
public abstract class PlayerBaseState : State
{   
    // !!!!!! 每个角色状态都需要访问状态机：调状态机的switchState()方法
    protected PlayerStateMachine stateMachine;

    // 构造器，需要传入一个角色状态机
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // 不管状态如何变化，需要处理同一个受力对象：如重力、受击
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
}
