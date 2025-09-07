using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzhuangController : MonoBehaviour
{
    
    public void BeHit()
    {
        EffectUIController.Instance.ShowEnemyEffectUI(gameObject, 200);
    }
}
