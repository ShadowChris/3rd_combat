using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    /**
     * 相机锁定的目标组：旨在将目标的方位加入相机组，从而实现动态调整锁定目标后的相机视角
     */
    [field: SerializeField] private CinemachineTargetGroup cineTargetGroup;
    
    /**
     * targets：保存进入领域范围的target列表
     */
    [field: SerializeField] private List<Target> targets = new List<Target>();

    /**
     * 保存当前锁定的目标
     */
    public Target CurrentTarget { get; private set; }


    /**
     * OnTriggerEnter(Collider other)：一个target刚进入领域，触发该函数
     */
    private void OnTriggerEnter(Collider other)
    {
        // 校验t是非空后，加入列表
        Target t = other.GetComponent<Target>();
        if (t == null) return;
        targets.Add(t);
        // 另一种写法：
        // if (!other.TryGetComponent<Target>(out Target target)) { return; }
        //targets.Add(t);

        //t.Ondestroyed触发时，会执行下方移除目标的方法
        t.Ondestroyed += RemoveTarget;
    }

    /*
     * OnTriggerExit(Collider other)：一个target刚退出领域，触发该函数
     */
    private void OnTriggerExit(Collider other)
    {
        Target t = other.GetComponent<Target>();
        if (t == null) return;
        // targets.Remove(t);
        // 另一种写法：
        // if (!other.TryGetComponent<Target>(out Target target)) { return; }
        //targets.Remove(t);
        
        RemoveTarget(t);
    }

    /*
     * SelectTarget()：锁定目标
     */
    public bool SelectTarget()
    {   // 没有目标，不锁定
        if (targets.Count == 0) { return false; }
        
        // 将列表第一个元素作为锁定的目标
        CurrentTarget = targets[0];
        
        // 将该目标加入相机组
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        
        return true;
    }

    /*
     * Cancel()：取消锁定
     */
    public void Cancel()
    {
        if (CurrentTarget == null) { return; }
        
        // 如果按了esc键，取消锁定，移除相机组的target元素。
        // 但是有一个问题：目标摧毁时，如何移除元素？答：target发送信号，触发RemoveTarget()方法
        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    /**
     * 
     */
    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            // 移除"相机组"的目标元素
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        target.Ondestroyed -= RemoveTarget;
        
        // 移除"目标列表"的目标元素
        targets.Remove(target);
    }
}