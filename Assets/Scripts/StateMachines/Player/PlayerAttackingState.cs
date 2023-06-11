using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{

    private float previousFrameTime;
    /**
     * ��ǰ��������
     */
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        this.attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        // CrossFadeInFixedTime()���ö������ɵø���˳����fixedTransionDuration: ���������Ĺ���ʱ��
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        // 1. ������ʱ�ƶ��߼�����������ʱ�������ɫ�뿪���棬��Ҫ����ɫʩ��һ�����������������ڿ���
        Move(deltaTime);

        //Debug.Log(stateMachine.InputReader.IsAttacking);
        //-----------------------------------------
        // 2. ���������߼�
        float normalizedTime = GetNormalizedTime();
        
        // TO DO: ������ɱ߽�
        if (normalizedTime > previousFrameTime && normalizedTime < 1f)
        {
            // ������״̬
            if (stateMachine.InputReader.IsAttacking)
            {
                Debug.Log("@@@TryComboAttack@@@");
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // go back to locomotion
            //stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {

    }

    // ��������
    private void TryComboAttack(float normalizedTime)
    {
        //Debug.Log(stateMachine.InputReader.IsAttacking);
        if (attack.ComboStateIndex == -1) { return; }

        Debug.Log(string.Format("Name:{0}, NormalizedTime: {1}, ComboAttackTime: {2}", attack.AnimationName, normalizedTime, attack.ComboAttackTime));
        // ������Ҫ������һ��ʱ����ܴ�����һ�ι���
        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
        
    }

    

    private float GetNormalizedTime()
    {
        // ��ǰ����״̬��Ϣ
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        // ��һ������״̬��Ϣ
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);


        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            // 1. ��������������Ĺ��ɽ׶Σ�������һ��������ʱ��
            //Debug.Log("Next time: " + nextInfo.normalizedTime);
            return nextInfo.normalizedTime;

        } else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            //2. ������ڹ��ɽ׶Σ�����ĳ�������м䣩���򷵻ص�ǰ�Ķ���ʱ��
            //Debug.Log("Current time: " + currentInfo.normalizedTime);
            return currentInfo.normalizedTime;

        } else
        {
            // ���ǹ���״̬����
            return 0f;
        }
        // �������⣺�����ǰһ�����������һ֡��ȡ��Ϣ�����������⣨����
    }


}
