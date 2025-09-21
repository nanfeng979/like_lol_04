using System.Collections;
using System.Collections.Generic;
using LikeLoL04;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayerInfoController : MonoBehaviour
{
    public static OtherPlayerInfoController Instance;

    public CanvasGroup canvasGroup;

    private bool isVisible = false;
    [SerializeField]
    private float autoHideSeconds = 3f; // X 秒后自动隐藏；<=0 表示不自动隐藏
    private float hideAtTime = -1f;

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

        Hide();
    }

    void Start()
    {
        ClickInteractionManager.Instance.OnLeftClickWithTarget += ShowOtherPlayerInfo;
    }

    void Update()
    {
        if (isVisible && autoHideSeconds > 0f && hideAtTime > 0f)
        {
            if (Time.time >= hideAtTime)
            {
                Hide();
            }
        }
    }

    void OnDestroy()
    {
        ClickInteractionManager.Instance.OnLeftClickWithTarget -= ShowOtherPlayerInfo;
    }

    private void ShowOtherPlayerInfo(LOLGameObject target)
    {
        if (target != null)
        {
            AvatarImage.sprite = target.Data.Avatar;
            Show();
        }
    }

    public Image AvatarImage;

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        isVisible = true;
        if (autoHideSeconds > 0f)
        {
            hideAtTime = Time.time + autoHideSeconds; // 重置计时
        }
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        isVisible = false;
        hideAtTime = -1f;
    }

}
