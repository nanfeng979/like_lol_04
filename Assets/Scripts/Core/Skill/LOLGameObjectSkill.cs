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

    public void UpgradeSkill(int slot)
    {
        switch (slot)
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
                Debug.LogWarning($"Invalid skill slot: {slot}");
                break;
        }
    }

    public bool IsSkillLearned(int slot)
    {
        switch (slot)
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
                Debug.LogWarning($"Invalid skill slot: {slot}");
                return false;
        }
    }
}
