using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{   
    /**
     * 当前攻击动作
     */
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackId) : base(stateMachine)
    {
        this.attack = stateMachine.Attacks[attackId];
    }

    public override void Enter()
    {
        // CrossFadeInFixedTime()：让动画过渡得更加顺畅。fixedTransiionDuration: 两个动画得过渡时间
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, 0.1f);
    }
    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }


}
