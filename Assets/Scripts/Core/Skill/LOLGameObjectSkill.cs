using UnityEngine;

public class LOLGameObjectSkill
{
    private int skill1Level = 0;
    private int skill2Level = 0;
    private int skill3Level = 0;
    private int skill4Level = 0;

    public int Skill1Level => skill1Level;
    public int Skill2Level => skill2Level;
    public int Skill3Level => skill3Level;
    public int Skill4Level => skill4Level;

    public void UpgradeSkill(int levelType)
    {
        switch (levelType)
        {
            case 1:
                skill1Level++;
                break;
            case 2:
                skill2Level++;
                break;
            case 3:
                skill3Level++;
                break;
            case 4:
                skill4Level++;
                break;
            default:
                Debug.LogWarning($"Invalid skill levelType: {levelType}");
                break;
        }
    }

    public bool IsSkillLearned(int levelType)
    {
        switch (levelType)
        {
            case 1:
                return skill1Level > 0;
            case 2:
                return skill2Level > 0;
            case 3:
                return skill3Level > 0;
            case 4:
                return skill4Level > 0;
            default:
                Debug.LogWarning($"Invalid skill levelType: {levelType}");
                return false;
        }
    }

    public int GetSkillLevel(int levelType)
    {
        switch (levelType)
        {
            case 1:
                return skill1Level;
            case 2:
                return skill2Level;
            case 3:
                return skill3Level;
            case 4:
                return skill4Level;
            default:
                Debug.LogWarning($"Invalid skill levelType: {levelType}");
                return -1;
        }
    }

    public int GetSkillMaxLevel(int levelType)
    {
        switch (levelType)
        {
            case 1:
                return 5;
            case 2:
                return 5;
            case 3:
                return 5;
            case 4:
                return 3;
            default:
                Debug.LogWarning($"Invalid skill levelType: {levelType}");
                return -1;
        }
    }

    public int UpgradeSkill(int levelType, int addLevel)
    {
        switch (levelType)
        {
            case 1:
                if (skill1Level + addLevel <= GetSkillMaxLevel(levelType))
                {
                    skill1Level += addLevel;
                    return skill1Level;
                }
                break;
            case 2:
                if (skill2Level + addLevel <= GetSkillMaxLevel(levelType))
                {
                    skill2Level += addLevel;
                    return skill2Level;
                }
                break;
            case 3:
                if (skill3Level + addLevel <= GetSkillMaxLevel(levelType))
                {
                    skill3Level += addLevel;
                    return skill3Level;
                }
                break;
            case 4:
                if (skill4Level + addLevel <= GetSkillMaxLevel(levelType))
                {
                    skill4Level += addLevel;
                    return skill4Level;
                }
                break;
            default:
                Debug.LogWarning($"Invalid skill levelType: {levelType}");
                break;
        }

        return -1;
    }
}
