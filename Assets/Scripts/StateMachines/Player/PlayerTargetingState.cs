using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    // 锁定目标状态的动画混合树
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        // 将本类下方的onCancel()注册到Event中。Event被触发，则触发响应函数
        stateMachine.InputReader.CancelEvent += OnCancel;
        // 过渡到当前状态动画
        stateMachine.Animator.Play(TargetingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        // 如果丢失目标（离开目标范围），自动转换状态
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
        
        FaceTarget();
        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
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

}
