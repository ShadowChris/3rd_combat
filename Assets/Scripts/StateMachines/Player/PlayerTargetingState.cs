using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    // 锁定目标状态的动画混合树
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float TargetingAnimatorDampTime = 0.1f;
    /**
     * 其他动画切换到本锁定动画的过渡时间
     */
    private const float CrossFadeDuration = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        // 将本类下方的onCancel()注册到Event中。Event被触发，则触发响应函数
        stateMachine.InputReader.CancelEvent += OnCancel;
        // 过渡到当前状态动画
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // 首先检查是否按下攻击键
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        // 如果丢失目标（离开目标范围），自动转换状态
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
        
        FaceTarget();
        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelEvent -= OnCancel;
    }

    private void OnCancel()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
    
    private Vector3 CalculateMovement()
    {
        // 根据角色朝向位置调整移动视角。角色朝向锁定至目标(在PlayerBaseState的FaceTarget())，相机角度不需要
        Transform playerTransform = stateMachine.transform;
        
        Vector3 forward = playerTransform.forward;
        Vector3 right = playerTransform.right;
        
        return forward * stateMachine.InputReader.MovementValue.y +
               right * stateMachine.InputReader.MovementValue.x;
    }

    private void UpdateAnimator(float deltaTime)
    {
        // 前后移动动画过渡
        switch (stateMachine.InputReader.MovementValue.y)
        {
            case 0:
                stateMachine.Animator.SetFloat(TargetingForwardHash, 0, TargetingAnimatorDampTime, deltaTime);
                break;
            case > 0:
                stateMachine.Animator.SetFloat(TargetingForwardHash, 1, TargetingAnimatorDampTime, deltaTime);
                break;
            default:
                stateMachine.Animator.SetFloat(TargetingForwardHash, -1, TargetingAnimatorDampTime, deltaTime);
                break;
        }
        
        // 左右移动动画过渡
        switch (stateMachine.InputReader.MovementValue.x)
        {
            case 0:
                stateMachine.Animator.SetFloat(TargetingRightHash,0, TargetingAnimatorDampTime, deltaTime);
                break;
            case > 0:
                stateMachine.Animator.SetFloat(TargetingRightHash, 1, TargetingAnimatorDampTime, deltaTime);
                break;
            default:
                stateMachine.Animator.SetFloat(TargetingRightHash, -1, TargetingAnimatorDampTime, deltaTime);
                break;
        }

    }

}
