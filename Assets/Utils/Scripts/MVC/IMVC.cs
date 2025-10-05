/// <summary>
/// 控制器接口，定义所有控制器需要的统一方法
/// </summary>
public interface IMVC_Controller
{
    // void Init();     // 初始化
    // void Show();     // 显示
    // void Hide();     // 隐藏
}

/// <summary>
/// View接口，主要用于UI交互
/// </summary>
public interface IMVC_View
{
    void Show();
    void Hide();
    void ToggleShow();
    // void Refresh(); // 刷新显示内容
}

/// <summary>
/// Model接口，主要用于数据管理
/// </summary>
public interface IMVC_Model
{
    // void Reset();   // 重置数据
}
