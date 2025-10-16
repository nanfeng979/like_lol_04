using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLearnModel : IMVC_Model
{
    private LOLGameObjectSkill m_mainPlayerSkill;

    public LOLGameObjectSkill MainPlayerSkill
    {
        get { return m_mainPlayerSkill; }
        set { m_mainPlayerSkill = value; }
    }
}
