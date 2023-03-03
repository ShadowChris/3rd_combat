using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTestState: PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {   
        // // 把下面的onjump方法注册到JumpEvent中
        // stateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Tick(float deltaTime)
    {
        // 获取控制器的3D坐标
        Vector3 movement = new Vector3();
        movement.x = stateMachine.InputReader.MovementValue.x;
        movement.y = 0; // y是垂直坐标
        movement.z = stateMachine.InputReader.MovementValue.y;

        // // player根据movement坐标进行平移（无视障碍物）；乘deltaTime排除帧率对移动速度的影响
        // stateMachine.transform.Translate(movement * deltaTime);
        
        // 更优：使用Controller控制角色移动,同时设置速度
        stateMachine.Controller.Move(movement * (deltaTime * stateMachine.FreeLookMovementSpeed));

    }

    public override void Exit()
    {
        // stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        // 需要引用父类的状态机，调用方法实现状态转移
        stateMachine.SwitchState(new PlayerTestState(stateMachine));
    }
}
