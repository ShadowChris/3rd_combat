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
        
        // 功能： 控制角色移动
        Vector3 movement = CalculateMovement();
        Debug.Log(movement);
        
        //*********************************************************************
        // player根据movement坐标进行平移（无视障碍物）；乘deltaTime排除帧率对移动速度的影响
        // stateMachine.transform.Translate(movement * deltaTime);
        
        // 更优：使用CharacterController控制角色移动,同时设置速度
        stateMachine.Controller.Move(movement * (deltaTime * stateMachine.FreeLookMovementSpeed));
        
        //动画过渡
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            // 修改animator的参数。
            // name: 需要修改的参数名，value: 状态的参数值(越靠近0：静止，越靠近1：跑步)，
            // dampTime：让状态平滑过渡的时间
            stateMachine.Animator.SetFloat("FreeLookSpeed", 0, 0.1f, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat("FreeLookSpeed", 1, 0.1f, deltaTime);
        
        
        // 移动的时候，需要控制角色的朝向.Quaternion: 四元组，rotation的对象
        stateMachine.transform.rotation = Quaternion.LookRotation(movement);
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

    private Vector3 CalculateMovement()
    {
        // 获取控制器的3D坐标
        // Vector3 movement = new Vector3();
        // movement.x = stateMachine.InputReader.MovementValue.x;
        // movement.y = 0; // y是垂直坐标
        // movement.z = stateMachine.InputReader.MovementValue.y;
        
        // 更优：根据相机位置调整移动视角
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;
        
        // 人物移动不需要相机y轴的倾斜角，因此归零
        forward.y = 0;
        right.y = 0;
        return forward * stateMachine.InputReader.MovementValue.y +
               right * stateMachine.InputReader.MovementValue.x;
    }
    
}
