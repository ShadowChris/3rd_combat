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

    protected void Move(float deltaTime)
    {
        return;
    }

}
