using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    // 动画器：控制角色的动画效果
    [field: SerializeField] public Animator Animator { get; private set; }
    
}
