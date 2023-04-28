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
    [field: SerializeField] public string AnimationName { get; private set; }
}
