using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// LOL客户端控制器
    /// </summary>
    public class LOLClientController : MonoBehaviour
    {

        public static LOLClientController Instance { get; private set; }


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
