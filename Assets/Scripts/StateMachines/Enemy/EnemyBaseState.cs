using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    
    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected bool IsInChaseRange() {
        // 判断玩家是否在追踪范围内
        return Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position) < stateMachine.PlayerChasingRange;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {   
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
    // 不做任何运动，只施加重力
    protected void Move(float deltaTime) {
        Move(Vector3.zero, deltaTime);
    }

    // 需要在chasing state和attack state中使用到
    protected void FacePlayer() {
        if (stateMachine.Player == null) return;
        Vector3 lookPos = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
        lookPos.y = 0;
        // 面向玩家
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

}
