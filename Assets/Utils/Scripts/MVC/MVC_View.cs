using UnityEngine;

public class MVC_View : MonoBehaviour, IMVC_View
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void ToggleShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
