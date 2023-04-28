using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{   
    /**
     * ��ǰ��������
     */
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackId) : base(stateMachine)
    {
        this.attack = stateMachine.Attacks[attackId];
    }

    public override void Enter()
    {
        // CrossFadeInFixedTime()���ö������ɵø���˳����fixedTransiionDuration: ���������ù���ʱ��
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, 0.1f);
    }
    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }


}
