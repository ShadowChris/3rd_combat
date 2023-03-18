using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    // targets�������������Χ��target�б�
    public List<Target> targets = new List<Target>();

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
}
