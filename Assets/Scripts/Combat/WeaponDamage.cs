using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    /**
     * 持有当前武器的角色的碰撞体，防止自己的武器碰撞到角色自己
     */
    [SerializeField] private Collider myCollider;
    
    private int damage = -10;

    /**
     * 保存已经碰撞过的碰撞体，避免同一帧触发多次碰撞
     */
    private List<Collider> alreadyCollidedWith = new List<Collider>();

    /**
     * 武器碰撞机制唤醒时，重置已经碰撞过的碰撞体列表
     */
    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果碰撞体是玩家自己，或已经在已经碰撞过的碰撞体列表，直接返回
        if (other == myCollider) return;
        if (alreadyCollidedWith.Contains(other)) return;

        // 将碰撞体添加到已经碰撞过的碰撞体列表中 
        alreadyCollidedWith.Add(other);

        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.DealDamage(damage);
        }

    }

    /**
     * 设置招式的攻击力
     */
    public void setAttack(int damage) {
        this.damage = damage;
    }
}
