using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUIController : MonoBehaviour
{
    public static EffectUIController Instance;

    public CanvasGroup enemyEffectCanvasGroup;

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
    
    public void Start()
    {
        HideEnemyEffectUI();
    }

    public void ShowEnemyEffectUI(GameObject gameObject, float offsetY = 0, float damage = 0)
    {
        enemyEffectCanvasGroup.alpha = 1;
        enemyEffectCanvasGroup.blocksRaycasts = true;
        enemyEffectCanvasGroup.interactable = true;

        GameObjectPositionToCanvasPosition(gameObject, out Vector3 canvasPos);
        canvasPos.y += offsetY;
        enemyEffectCanvasGroup.GetComponent<RectTransform>().position = canvasPos;

        enemyEffectCanvasGroup.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = damage.ToString();

        StartCoroutine(HideEnemyEffectUIAfterSeconds(0.35f));
    }

    private IEnumerator HideEnemyEffectUIAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HideEnemyEffectUI();
    }

    public void HideEnemyEffectUI()
    {
        enemyEffectCanvasGroup.alpha = 0;
        enemyEffectCanvasGroup.blocksRaycasts = false;
        enemyEffectCanvasGroup.interactable = false;
    }
    
    public void GameObjectPositionToCanvasPosition(GameObject gameObject, out Vector3 canvasPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(enemyEffectCanvasGroup.GetComponent<RectTransform>(), screenPos, null, out canvasPos);
    }
    
}
