using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * InputReader类: 处理Unity中输入系统（InputSystem），将其映射到StateMachine具体的功能中
 * Controls.IPlayerActions：输入系统-玩家动作的接口，需要在此类实现
 */
public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    // 人物动作移动：+x：右，-x：左，+y前，-y后
    public Vector2 MovementValue { get; private set; }
    
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action CancelEvent;

    // 检测玩家是否按了攻击键
    public bool IsAttacking { get; private set; }

    // unity输入系统（InputSystem）映射的控制类
    private Controls controls;

    /**
     * 开始监听
     */
    private void Start()
    {
        controls = new Controls();
        // 将新建的controls对象和本类的OnJump()进行关联
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    /**
     * 游戏结束，结束监听，销毁此对象
     */
    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    /**
     * 实现方法OnJump: 当玩家按下空格键或者手柄的south键（在unity中的InputSystem可以调整），执行此方法
     */
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        // 如果按下按键，会触发JumpEvent信号。监听jumpEvent的地方就会接收到
        JumpEvent?.Invoke();
    }

    /**
     * 闪避
     */
    public void OnDodge(InputAction.CallbackContext context)
    {
        if(!context.performed) {return;}
        DodgeEvent?.Invoke();
    }
    
    /**
     * 监听控制器移动，按响应键盘按钮会触发以下函数。
     * 参数context可以获取三维坐标，然后赋值给InputReader的movementValue
     */
    public void OnMove(InputAction.CallbackContext context)
    {
        // context会读取动作的vector坐标
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //cinemachine为我们处理了look方法，因此不需要在函数内实现逻辑
    }
    /**
     * OnTarget/OnCancel: 锁定/取消目标
     */
    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        TargetEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        CancelEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) { IsAttacking = true; } 
        else if (context.canceled) { IsAttacking = false; }
    }
}