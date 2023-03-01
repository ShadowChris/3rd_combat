using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTestState: PlayerBaseState
{
    private float timer;
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {   
        // 把下面的onjump方法注册到JumpEvent中
        stateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Tick(float deltaTime)
    {
        timer += deltaTime;
        Debug.Log(timer);
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        // 需要引用父类的状态机，调用方法实现状态转移
        stateMachine.SwitchState(new PlayerTestState(stateMachine));
    }
}
