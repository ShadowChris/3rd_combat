using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    protected void Move(float deltaTime)
    {
        // 只施加重力，无运动的向量。用在非移动状态中（如攻击等）
        Move(Vector3.zero, deltaTime);
    }

    // 面部朝向目标
    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) return;
        
        Vector3 otherTarget = stateMachine.Targeter.CurrentTarget.transform.position;
        Vector3 ourTarget = stateMachine.transform.position;
        Vector3 lookPos = otherTarget - ourTarget;
        lookPos.y = 0f;
        // 角色锁定目标物体，不跟相机视角移动了（虽然targeting里的calculateMovement()也没写根据相机视角移动）
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }
}
