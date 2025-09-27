using System.Collections;
using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 玩家信息系统，负责管理玩家信息的整体功能
    /// </summary>
    public class PlayerInfoSystem : MonoBehaviour
    {
        public static PlayerInfoSystem instance;

        public PlayerInfoController playerInfoController;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

    }
}
