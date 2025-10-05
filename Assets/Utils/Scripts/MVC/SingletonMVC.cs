using UnityEngine;

/// <summary>
/// 通用 MVC 单例控制器基类
/// </summary>
/// <typeparam name="TController">控制器类型（继承自己）</typeparam>
/// <typeparam name="TView">视图类型</typeparam>
/// <typeparam name="TModel">数据模型类型</typeparam>
public abstract class SingletonMVC<TController, TView, TModel> : Singleton<TController>
    where TController : SingletonMVC<TController, TView, TModel>
    where TView : Component
    where TModel : new()
{
    [Header("MVC Components")]
    [SerializeField] protected TView view;
    protected TModel model;

    protected override void Awake()
    {
        base.Awake();

        // 初始化 Model
        if (model == null)
        {
            model = new TModel();
        }

        // 如果没有手动挂 View，就自动找
        if (view == null)
        {
            view = GetComponentInChildren<TView>(true);
        }
    }
}
