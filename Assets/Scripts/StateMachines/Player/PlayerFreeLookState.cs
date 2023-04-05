using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFreeLookState: PlayerBaseState
{
    /**
     * 1. 统一管理传入Animator的参数名；
     * 2. 使用readonly代替const：const在编译前就不可更改，但是Animator.StringToHash()是在编译后赋值。因此使用readonly：被第一次赋值则不可更改
     * 3. 使用int的类型Hash传入Animator的速度比直接传入字符串更快
     */
    // 闲置状态速度
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    // 闲置状态的动画混合树
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    /**
     * 角色动画动作之间的过渡时间
     */
    private const float AnimatorDampTime = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        // 把下面的OnXXX()方法注册到XXXEvent中：如果XXXEvent被触发，就会执行OnXXX()函数
        stateMachine.InputReader.TargetEvent += OnTarget;
        // stateMachine.InputReader.JumpEvent += OnJump;
        
        // 切换到闲置动画
        stateMachine.Animator.Play(FreeLookBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        
        // 功能： 控制角色移动
        Vector3 movement = CalculateMovement();
        //Debug.Log(movement);
        
        //*********************************************************************
        // player根据movement坐标进行平移（无视障碍物）；乘deltaTime排除帧率对移动速度的影响
        // stateMachine.transform.Translate(movement * deltaTime);
        
        // 更优：使用CharacterController控制角色移动,同时设置速度
        // stateMachine.Controller.Move(movement * (deltaTime * stateMachine.FreeLookMovementSpeed));
        
        // 更更优：使用BaseState的Move()方法，首先为角色运动赋予一个全局的力，例如重力
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
        
        // 动画过渡
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            // 修改animator的参数。
            // name: 需要修改的参数名，value: 状态的参数值(越靠近0：静止，越靠近1：跑步)，
            // dampTime：让状态平滑过渡的时间
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        // 调整角色的朝向
        FaceMovementDirection(movement, deltaTime);
        
    }


    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        // stateMachine.InputReader.JumpEvent -= OnJump;
    }


    //-----------------------------------------------------
    // OnXXX()函数：状态开始时被注册到InputReader里的event事件中。
    // 按下对应按钮，就会invoke事件，从而执行相应的OnXXX()函数
    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) { return; }

        // 切换到锁定目标状态
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private void OnJump()
    {
        // 需要引用父类的状态机，调用方法实现状态转移
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
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

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        // 移动的时候，需要控制角色的朝向.Quaternion: 四元组，rotation的对象
        //stateMachine.transform.rotation = Quaternion.LookRotation(movement);

        // 优化：引入丝滑程度，使得旋转的时候更加流畅丝滑，而不是瞬间切换
        // Quaternion.Lerp(a,b,t): 线性插值法，在[a,b]之间取一个值，t∈[0,1]。t越小，越接近a；t越大，越接近b 
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping
        );

    }

}
