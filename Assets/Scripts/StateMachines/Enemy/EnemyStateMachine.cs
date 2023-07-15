using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    // 动画器：控制角色的动画效果
    [field: SerializeField] public Animator Animator { get; private set; }

    // 角色控制器：控制角色的移动等属性，涉及角色的碰撞参数（内嵌类，详情见Inspector）
    [field: SerializeField] public CharacterController Controller { get; private set; }

    // 受力器：实现角色的各种受力状况，如：重力、反击
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    // 导航代理：实现角色的寻路
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }

    // 寻路范围：设置触发自动追踪玩家的距离
    [field: SerializeField] public float PlayerChasingRange { get; private set; }

    // 敌人移动速度
    [field: SerializeField] public float ChasingMovementSpeed { get; private set; }

    // 旋转丝滑程度：值越大，人物动作旋转越丝滑
    [field: SerializeField] public float RotationDamping { get; private set; }


    // 保存玩家的组件
    public GameObject Player { get; private set; }

    private void Start()
    {

        // 搜索玩家的gameobject
        Player = GameObject.FindGameObjectWithTag("Player");

        SwitchState(new EnemyIdleState(this));

    }
    private void OnDrawGizmosSelected()
    {
        // 绘制玩家追踪范围，选中就会显示
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }


}
