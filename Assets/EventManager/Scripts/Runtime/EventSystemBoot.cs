using UnityEngine;

namespace LikeLoL04.EventSystem
{
    /// <summary>
    /// 事件系统启动组件：尽早初始化 EventBus。
    /// 将该脚本放到场景首个加载的对象上，或勾选 "自动创建引导对象"。
    /// </summary>
    [DefaultExecutionOrder(-10000)]
    public sealed class EventSystemBoot : MonoBehaviour
    {
        [Header("是否在启动时自动创建全局引导对象")]
        [Tooltip("若场景中没有主动放置该组件，可开启此项以便在第一次加载时自动创建并初始化事件系统。")]
        [SerializeField] private bool autoCreateBootstrap = true;

        [Header("是否在场景切换间持久化")]
        [Tooltip("是否将该引导对象标记为 DontDestroyOnLoad。")]
        [SerializeField] private bool dontDestroyOnLoad = true;

        private static bool s_Created;

        private void Awake()
        {
            // 初始化总线
            EventBus.Initialize();

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoBootstrap()
        {
            if (s_Created)
            {
                return;
            }

            // 确保事件总线在场景加载前已初始化
            EventBus.Initialize();

            // 如果项目没有显式放置，引导一次
            GameObject existing = GameObject.Find("[EventSystemBoot]");
            if (existing != null && existing.GetComponent<EventSystemBoot>() != null)
            {
                s_Created = true;
                return;
            }

            GameObject go = new GameObject("[EventSystemBoot]");
            EventSystemBoot boot = go.AddComponent<EventSystemBoot>();
            if (!boot.autoCreateBootstrap)
            {
                // 用户关闭了自动创建，则销毁并返回
                Object.Destroy(go);
                return;
            }

            s_Created = true;
        }
    }
}
