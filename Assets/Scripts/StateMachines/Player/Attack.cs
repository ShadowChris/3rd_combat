using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ���湥�������Ķ������ƣ���Ҫע������л�
 */
[Serializable]
public class Attack
{
    // ��ǰ���������Ķ�����
    [field: SerializeField] public string AnimationName { get; private set; }

    // 2������֮��Ĺ���ʱ��
    [field: SerializeField] public float TransitionDuration { get; private set; }

    // ����״̬index��Ĭ��-1���ǹ���״̬
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;

    // ���еĹ�������ʱ��
    [field: SerializeField] public float ComboAttackTime { get; private set; }
}
