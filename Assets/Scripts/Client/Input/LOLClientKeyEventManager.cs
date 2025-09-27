using System;
using UnityEngine;

/// <summary>
/// 监听 Shift+Q/W/E/R 的按键事件管理器。
/// 模式参考 LOLClientMouseEventManager：集中输入检测，其他系统订阅事件。
/// </summary>
[DefaultExecutionOrder(-50)]
public class LOLClientKeyEventManager : MonoBehaviour
{
    public static LOLClientKeyEventManager Instance { get; private set; }

    /// <summary>
    /// 当按下 Shift + (Q|W|E|R) 时触发，参数为技能槽位 1~4
    /// </summary>
    public event Action<int> OnShiftSkillKey;
    /// <summary>
    /// 当未按 Shift 时直接按下 (Q|W|E|R) 触发，参数为技能槽位 1~4（用于释放技能）
    /// </summary>
    public event Action<int> OnSkillKey;

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

    void Update()
    {
        HandleKeyDetection();
    }

    private void HandleKeyDetection()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // 处理四个键：如果按下同时按住 Shift -> 升级事件，否则 -> 施放事件
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (shiftHeld) OnShiftSkillKey?.Invoke(1); else OnSkillKey?.Invoke(1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (shiftHeld) OnShiftSkillKey?.Invoke(2); else OnSkillKey?.Invoke(2);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (shiftHeld)
            {
                OnShiftSkillKey?.Invoke(3);
            }
            else
            {
                OnSkillKey?.Invoke(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (shiftHeld) OnShiftSkillKey?.Invoke(4); else OnSkillKey?.Invoke(4);
        }
    }
}
