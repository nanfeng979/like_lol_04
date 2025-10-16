using System;
using System.Collections;
using System.Collections.Generic;
using LikeLoL04;
using UnityEngine;

public class MainPlayerSkillSystem : MonoBehaviour
{

    public static MainPlayerSkillSystem Instance;

    public LOLHeroGameObject mainPlayer;

    public event Action<int> OnUpgradeSkill;

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

    void OnEnable()
    {
        TrySubscribe();
    }

    void Start()
    {
        // 再尝试一次，防止脚本执行顺序导致的首次错过
        TrySubscribe();
    }

    void OnDisable()
    {
        if (LOLClientKeyEventManager.Instance != null)
        {
            LOLClientKeyEventManager.Instance.OnShiftSkillKey -= HandleShiftSkillKey;
        }
    }

    private void TrySubscribe()
    {
        if (LOLClientKeyEventManager.Instance != null)
        {
            // 先取消再订阅，避免重复
            LOLClientKeyEventManager.Instance.OnShiftSkillKey -= HandleShiftSkillKey;
            LOLClientKeyEventManager.Instance.OnShiftSkillKey += HandleShiftSkillKey;
        }
    }

    private void HandleShiftSkillKey(int slot)
    {
        OnUpgradeSkill?.Invoke(slot);
    }
}
