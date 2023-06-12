using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 储存攻击动作的动画名称，需要注解可序列化
 */
[Serializable]
public class Attack
{
    // 当前攻击动作的动画名
    [field: SerializeField] public string AnimationName { get; private set; }

    // 2个动画之间的过渡时间
    [field: SerializeField] public float TransitionDuration { get; private set; }

    // 连击状态index：默认-1，非攻击状态
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;

    // 连招的攻击持续时间
    [field: SerializeField] public float ComboAttackTime { get; private set; }
    
    // --------------以下为攻击惯性属性---------------
    /**
    * 施加力的时间
    */
    [field: SerializeField] public float ForceTime { get; private set; }
    /**
    * 惯性力的大小
    */
    [field: SerializeField] public float Force { get; private set; }
}
