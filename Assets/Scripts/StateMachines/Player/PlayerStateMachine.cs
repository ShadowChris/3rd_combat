using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    // 具体状态机：传入状态（私有方法）
    private void Start()
    {
        SwitchState(new PlayerTestState(this));
    }
    
}
