using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    // 锁定目标状态的动画混合树
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

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

}
