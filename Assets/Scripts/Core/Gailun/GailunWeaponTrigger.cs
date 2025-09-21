using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 盖伦武器碰撞触发器
    /// </summary>
    public class GailunWeaponTrigger : MonoBehaviour
    {
        public LOLGameObject Owner { get; set; }

        void Start()
        {
            enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Owner == null)
            {
                return;
            }

            LOLGameObject target = other.GetComponent<LOLGameObject>();
            if (target != null && target != Owner)
            {
                Debug.Log($"GailunWeaponTrigger: {Owner.name} 攻击了 {target.name}");
                // target.BeAttack();
            }
        }
    }
}
