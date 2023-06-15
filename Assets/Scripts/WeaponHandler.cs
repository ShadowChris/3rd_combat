using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    /**
    * 我们需要在动画事件中调用武器响应方法。
    * 但是，本脚本需要放在角色的根结点下才能在动画事件触发；
    * 武器又是角色的子物体，所以需要一个中间脚本来调用武器的方法。
    */
    [SerializeField] private GameObject weaponLogic;
    public void EnableWeapon() {
        weaponLogic.SetActive(true);
    }

    public void DisableWeapon() {
        weaponLogic.SetActive(false);
    }
}
