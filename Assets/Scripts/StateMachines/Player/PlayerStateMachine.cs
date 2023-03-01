using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{   
    // 输入监听器（Property属性）：设置为只读，私有类才能修改
    // [field: SerializeField]：把Property转成Field，就可以在unity检查器中获取
    [field: SerializeField] public InputReader InputReader { get; private set; }

    // 具体状态机：传入状态（私有方法）
    private void Start()
    {
        SwitchState(new PlayerTestState(this));
    }
    
}