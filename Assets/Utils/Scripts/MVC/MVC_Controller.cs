using UnityEngine;

/// <summary>
/// 通用 MVC 控制器基类
/// 继承自 SingletonMVC 并实现 IMVC_Controller 接口
/// </summary>
public abstract class MVC_Controller<TController, TView, TModel>
    : SingletonMVC<TController, TView, TModel>, IMVC_Controller
    where TController : MVC_Controller<TController, TView, TModel>
    where TView : Component, IMVC_View
    where TModel : IMVC_Model, new()
{
    public virtual void Init()
    {
        // model.Reset();
        // view.Refresh();
    }

    public virtual void Show()
    {
        // view.Show();
    }

    public virtual void Hide()
    {
        view.Hide();
    }

    protected virtual bool ViewDefaultActive => true;

    protected override void Awake()
    {
        base.Awake();
        if (ViewDefaultActive)
        {
            view.Show();
        }
        else
        {
            view.Hide();
        }
    }

    public virtual void ToggleShow()
    {
        view.ToggleShow();
    }
}
