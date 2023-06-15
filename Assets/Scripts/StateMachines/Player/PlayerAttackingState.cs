using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{

    /**
     * 当前攻击动作
     */
    private Attack attack;

    private bool alreadyAppliedForce;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        this.attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        // CrossFadeInFixedTime()：让动画过渡得更加顺畅。fixedTransionDuration: 两个动画的过渡时间
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        // 1. 处理攻击时移动逻辑：当攻击的时候如果角色离开地面，需要给角色施加一个力，而不是悬浮在空中
        Move(deltaTime);

        //Debug.Log(stateMachine.InputReader.IsAttacking);
        //-----------------------------------------
        // 2. 处理连击逻辑：当攻击的时候，如果玩家按下了攻击键，就会触发连击；
        // 当在攻击动画播放完毕后仍然没触发连击，就会回到Idle状态
        float normalizedTime = GetNormalizedTime();
        if (normalizedTime < 1f)
        {
            // 2.1 处理攻击惯性
            if (normalizedTime >= attack.ForceTime) {
                TryApplyForce();
            }
            // 2.2 处理连招
            if (stateMachine.InputReader.IsAttacking)
            {
                // Debug.Log("@@@TryComboAttack@@@");
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // go back to locomotion
            if (stateMachine.Targeter.CurrentTarget != null) {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            } else {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
    }

    public override void Exit()
    {

    }

    // 尝试连击
    private void TryComboAttack(float normalizedTime)
    {
        //Debug.Log(stateMachine.InputReader.IsAttacking);
        if (attack.ComboStateIndex == -1) { return; }

        // Debug.Log(string.Format("Name:{0}, NormalizedTime: {1}, ComboAttackTime: {2}", attack.AnimationName, normalizedTime, attack.ComboAttackTime));
        // 攻击需要持续到一定时间才能触发下一次攻击
        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
        
    }

    // 尝试为攻击添加一个向前的力（攻击惯性）
    private void TryApplyForce() {
        if (alreadyAppliedForce) { return; }
        
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyAppliedForce = true;
    }

    private float GetNormalizedTime()
    {
        // 当前攻击状态信息
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        // 下一个攻击状态信息
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);


        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            // 1. 如果在两个动画的过渡阶段，返回下一个动画的时间
            //Debug.Log("Next time: " + nextInfo.normalizedTime);
            return nextInfo.normalizedTime;

        } else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            //2. 如果不在过渡阶段（处于某个动画中间），则返回当前的动画时间
            //Debug.Log("Current time: " + currentInfo.normalizedTime);
            return currentInfo.normalizedTime;

        } else
        {
            // 不是攻击状态动画
            return 0f;
        }
        // 存在问题：如果是前一个动画的最后一帧获取信息，则会出现问题（？）
    }




}
