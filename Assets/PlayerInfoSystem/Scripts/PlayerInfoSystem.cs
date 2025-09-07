using System.Collections;
using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;

public class PlayerInfoSystem : MonoBehaviour
{
    public static PlayerInfoSystem instance;

    private CanvasGroup canvasGroup;

    private bool isVisible = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        canvasGroup = GetComponent<CanvasGroup>();

        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isVisible = !isVisible;
            if (isVisible)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

}
