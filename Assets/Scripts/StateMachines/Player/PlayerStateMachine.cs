using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{   
    // 输入监听器（Property属性）：设置为只读，私有类才能修改
    // [field: SerializeField]：把Property转成Field，就可以在unity检查器中获取
    [field: SerializeField] public InputReader InputReader { get; private set; }
    
    // 角色控制器：控制角色的移动等属性，涉及角色的碰撞参数（详情见Inspector）
    [field: SerializeField] public CharacterController Controller { get; private set; }
    
    // 正常移动速度。不同状态可能移动速度不一样，后续可能有TargetMovementSpeed或者其他的
    [field:SerializeField] public float FreeLookMovementSpeed { get; private set; }
    
    // 动画器：控制角色的动画效果
    [field: SerializeField] public Animator Animator { get; private set; }

    // 旋转丝滑程度：值越大，人物动作旋转越丝滑
    [field: SerializeField] public float RotationDamping { get; private set; }


    // 主相机位置：为了控制器跟随相机视角改变（按住W，同时移动视角也可以左右移动）
    public Transform MainCameraTransform { get; private set; }



    // 具体状态机：传入状态（私有方法）
    private void Start()
    {
        // 主相机位置赋值
        if (Camera.main != null) MainCameraTransform = Camera.main.transform;
        
        SwitchState(new PlayerFreeLookState(this));
    }
    
}
