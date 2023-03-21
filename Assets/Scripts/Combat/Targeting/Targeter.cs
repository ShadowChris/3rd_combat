using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    // targets：保存进入领域范围的target列表
    [field: SerializeField] private List<Target> targets = new List<Target>();

    // 保存当前锁定的目标
    public Target CurrentTarget { get; private set; }


    // OnTriggerEnter(Collider other)：一个target刚进入领域，触发该函数
    private void OnTriggerEnter(Collider other)
    {
        // 校验t是非空后，加入列表
        Target t = other.GetComponent<Target>();
        if (t == null) return;
        targets.Add(t);
        // 另一种写法：
        // if (!other.TryGetComponent<Target>(out Target target)) { return; }
        //targets.Add(t);
    }

    // OnTriggerExit(Collider other)：一个target刚退出领域，触发该函数
    private void OnTriggerExit(Collider other)
    {
        Target t = other.GetComponent<Target>();
        if (t == null) return;
        targets.Remove(t);
        // 另一种写法：
        // if (!other.TryGetComponent<Target>(out Target target)) { return; }
        //targets.Add(t);
    }

    // SelectTarget()：锁定目标
    public bool SelectTarget()
    {   // 没有目标，不锁定
        if (targets.Count == 0) { return false; }
        // 将列表第一个元素作为锁定的目标
        CurrentTarget = targets[0];
        return true;
    }

    // Cancel()：取消锁定
    public void Cancel()
    {
        CurrentTarget = null;
    }
}