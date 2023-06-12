using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ForceReceiver：用专门一个类来处理角色控制器 (Character Controller)的重力等其他力的问题，保证在切换状态的时候受力不变
 * 处理重力逻辑：当角色触地，向下速度置初始值。当不触地，施加重力
 */
public class ForceReceiver : MonoBehaviour
{
    /**
     * 需要处理角色控制器
     */
    [SerializeField] private CharacterController controller;
    /**
    * 力的过渡时间
    */
    [SerializeField] private float drag = 0.3f;
    
    /**
     * 受力的衰减速度
     */
    private Vector3 dampingVelocity;
    private Vector3 impact;

    /**
     * y轴的速度，代表重力。注意：重力是负数
     */
    private float verticalVelocity;
    
    /**
     * =>：相当于get()方法。返回角色的一个整体的全局受力情况
     */
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            // 注意：设0有小问题，就是走小斜坡的时候可能会不断触发重力状态：0f悬空，同时离开地面，就会触发falling
            // 解答：因此需要设置一个小值——保证一定程度上会贴着地面走，不会触发falling
            // (等待测试)
            // verticalVelocity = 0f;
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // 触发falling
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // 施加受力（连招带动身体的力；受击的力）
        // 原理：每帧都会调用Update()方法，不断改变impact的值，然后在下面的代码中，不断衰减impact的值
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        Debug.Log("impact: " + impact);
    }

    /**
     * 施加力
     */
    public void AddForce(Vector3 force) {
        impact += force;
    }
}