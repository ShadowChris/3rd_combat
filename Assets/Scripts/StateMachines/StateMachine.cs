using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 状态机：通过调用状态来实现状态转移
public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    // Update is called once per frame
    private void Update()
    {
        // 判断空操作符，等价于if(currentState == null)
        currentState?.Tick(Time.deltaTime);
    }
}
