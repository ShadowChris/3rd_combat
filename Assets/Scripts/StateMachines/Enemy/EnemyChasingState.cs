using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{

    /**
     * 1. 动画混合树的Hash值，用来处理动画之间的过渡
     */
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private const float CrossFadeDuration = 0.1f;

    /**
     * 2. 动画参数的Hash值，用来处理动画的播放
     */
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        
    }
    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange()) {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);
    }
    public override void Exit()
    {
        //停止寻路
        stateMachine.Agent.ResetPath();
        
    }

    private void MoveToPlayer(float deltaTime) {
        //自动跟踪玩家（两点距离，但是有坏处：不会避障）
        Vector3 movement = stateMachine.Player.transform.position - stateMachine.transform.position;
        // stateMachine.Controller.Move(movement * stateMachine.ChasingMovementSpeed * deltaTime);

        //寻路跟踪玩家（会避障）
        stateMachine.Agent.SetDestination(stateMachine.Player.transform.position);
        stateMachine.Controller.Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.ChasingMovementSpeed * deltaTime);
        // stateMachine.Agent.velocity = stateMachine.Controller.velocity;

        //目标旋转，总是朝着玩家
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping
        );
        
        stateMachine.Animator.SetFloat(SpeedHash, 1, AnimatorDampTime, deltaTime);
    }

}
