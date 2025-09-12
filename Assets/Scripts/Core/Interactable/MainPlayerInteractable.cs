using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 主玩家交互组件
    /// </summary>
    public class MainPlayerInteractable : MonoBehaviour
    {
        private IInteractable currentTarget;

        private LOLMainPlayer mainPlayer;

        void Start()
        {
            mainPlayer = GetComponent<LOLMainPlayer>();
        }

        void Update()
        {
            // 检查是否有交互目标
            if (currentTarget != null)
            {
                float distance = Vector3.Distance(transform.position, currentTarget.GetInteractPosition());
                if (distance <= currentTarget.GetRequiredDistance())
                {
                    // 到达交互点，执行交互
                    currentTarget.Interact(this);
                    currentTarget = null; // 清除目标
                }
            }
        }

        /// <summary>
        /// 移动到某个点
        /// </summary>
        public void MoveTo(Vector3 position)
        {
            mainPlayer.MoveToPosition(position);
        }

        /// <summary>
        /// 追击并攻击敌人
        /// </summary>
        public void Attack(IInteractable enemy)
        {
            Debug.Log("攻击敌人：" + enemy.GetLOLGameObject().Name);
            // TODO: 播放攻击动画、扣血逻辑
        }

        /// <summary>
        /// 设置当前交互目标
        /// </summary>
        public void SetInteractionTarget(IInteractable target)
        {
            currentTarget = target;
            MoveTo(target.GetInteractPosition());
        }
    }

}

