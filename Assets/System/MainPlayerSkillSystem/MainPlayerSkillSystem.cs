using System;
using System.Collections;
using System.Collections.Generic;
using LikeLoL04;
using UnityEngine;

public class MainPlayerSkillSystem : MonoBehaviour
{

    public static MainPlayerSkillSystem Instance;

    public LOLHeroGameObject mainPlayer;

    public event Action<int, int> OnUpgradeSkill;

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
        switch (slot)
        {
            case 1:
                mainPlayer.Skill.UpgradeSkill(1);
                OnUpgradeSkill?.Invoke(1, mainPlayer.Skill.Skill1Level);
                break;
            case 2:
                mainPlayer.Skill.UpgradeSkill(2);
                OnUpgradeSkill?.Invoke(2, mainPlayer.Skill.Skill2Level);
                break;
            case 3:
                mainPlayer.Skill.UpgradeSkill(3);
                OnUpgradeSkill?.Invoke(3, mainPlayer.Skill.Skill3Level);
                break;
            case 4:
                mainPlayer.Skill.UpgradeSkill(4);
                OnUpgradeSkill?.Invoke(4, mainPlayer.Skill.Skill4Level);
                break;
        }
    }
}
