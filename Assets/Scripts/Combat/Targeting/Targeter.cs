using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    // targets�������������Χ��target�б�
    [field: SerializeField] private List<Target> targets = new List<Target>();

    // ���浱ǰ������Ŀ��
    public Target CurrentTarget { get; private set; }


    // OnTriggerEnter(Collider other)��һ��target�ս������򣬴����ú���
    private void OnTriggerEnter(Collider other)
    {
        // У��t�Ƿǿպ󣬼����б�
        Target t = other.GetComponent<Target>();
        if (t == null) return;
        targets.Add(t);
        // ��һ��д����
        // if (!other.TryGetComponent<Target>(out Target target)) { return; }
        //targets.Add(t);
    }

    // OnTriggerExit(Collider other)��һ��target���˳����򣬴����ú���
    private void OnTriggerExit(Collider other)
    {
        Target t = other.GetComponent<Target>();
        if (t == null) return;
        targets.Remove(t);
        // ��һ��д����
        // if (!other.TryGetComponent<Target>(out Target target)) { return; }
        //targets.Add(t);
    }

    // SelectTarget()������Ŀ��
    public bool SelectTarget()
    {   // û��Ŀ�꣬������
        if (targets.Count == 0) { return false; }
        // ���б��һ��Ԫ����Ϊ������Ŀ��
        CurrentTarget = targets[0];
        return true;
    }

    // Cancel()��ȡ������
    public void Cancel()
    {
        CurrentTarget = null;
    }
}
