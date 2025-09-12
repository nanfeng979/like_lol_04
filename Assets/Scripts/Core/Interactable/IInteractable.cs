using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 可交互接口
    /// </summary>

    public interface IInteractable
    {
        /// <summary>
        /// 当玩家点击该对象时调用
        /// </summary>
        void Interact(MainPlayerInteractable player);

        /// <summary>
        /// 玩家与对象交互所需的最小距离
        /// </summary>
        float GetRequiredDistance();

        /// <summary>
        /// 玩家应该走到的位置
        /// （有的对象不一定是 transform.position，比如 NPC 面前一点）
        /// </summary>
        Vector3 GetInteractPosition();

        LOLGameObject GetLOLGameObject();
    }

}
