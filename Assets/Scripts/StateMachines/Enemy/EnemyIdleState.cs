using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    /**
     * 1. 动画混合树的Hash值，用来处理动画之间的过渡
     */
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private const float CrossFadeDuration = 0.1f;

    /**
     * 2. 动画参数的Hash值，用来处理动画的播放
     */
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;


    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {   
        // 其他动画切换到闲置动画
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        // 将本动画的Speed参数设置为0：设置0为初始值才是idle状态
        stateMachine.Animator.SetFloat(SpeedHash, 0f);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        
    }
}
