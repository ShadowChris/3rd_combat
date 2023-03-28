using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> Ondestroyed;

    // 如果当前对象被销毁（敌人被打败），则会调用OnDestroy()方法
    private void OnDestroy()
    {
        Ondestroyed?.Invoke(this);
        Debug.Log("Ondestroyed");
    }
}
