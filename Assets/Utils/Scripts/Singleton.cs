using UnityEngine;

/// <summary>
/// 通用单例基类
/// 使用方法：
/// 1. 继承此类，例如：public class GameManager : Singleton<GameManager> {}
/// 2. 可通过 _autoInstantiateInAwake 控制实例化方式。
///    - true: Awake 阶段自动实例化
///    - false: 第一次访问 Instance 时才实例化
/// </summary>
/// <typeparam name="T">要单例化的类类型</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // 单例实例
    private static T _instance;

    // 是否在 Awake 阶段自动实例化
    protected virtual bool AutoInstantiateInAwake => true;

    // 公共访问点
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 查找是否已有实例
                _instance = FindObjectOfType<T>();

                // 如果场景中没有，就创建一个新 GameObject
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (AutoInstantiateInAwake)
        {
            // 如果已有实例且不是自己，销毁重复的
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;
        }
    }
}
