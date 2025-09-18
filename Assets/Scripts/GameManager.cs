using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 游戏管理器，负责全局游戏状态和对象的管理
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public LOLMainPlayer mainPlayer;
    }
}
