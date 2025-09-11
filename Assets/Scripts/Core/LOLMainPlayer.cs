using UnityEngine;

public class LOLMainPlayer : LOLGameObject
{
    private Camera mainCamera;

    protected override void Start()
    {
        base.Start();
        mainCamera = Camera.main;
        Camp = new Camp { Type = CampType.Blue };
    }
    
    protected override void Update()
    {
        base.Update();
        HandleMouseDetection();
    }

    private void HandleMouseDetection()
    {
        // 确保LOLGameConfig实例存在
        if (LOLGameConfig.Instance == null) return;

        // 从鼠标位置发射射线
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 检测鼠标是否碰到物体
        if (Physics.Raycast(ray, out hit))
        {
            // 检查碰到的物体是否有LOLGameObject组件
            LOLGameObject hitGameObject = hit.collider.GetComponent<LOLGameObject>();

            if (hitGameObject != null)
            {
                // 检查阵营是否为敌对
                if (IsEnemyCamp(hitGameObject.Camp))
                {
                    // 切换为攻击光标
                    SetCursor(LOLGameConfig.Instance.AttackCursor);
                }
                else
                {
                    // 切换为默认光标
                    SetCursor(LOLGameConfig.Instance.DefaultCursor);
                }
            }
            else
            {
                // 没有LOLGameObject组件，使用默认光标
                SetCursor(LOLGameConfig.Instance.DefaultCursor);
            }
        }
        else
        {
            // 没有碰到任何物体，使用默认光标
            SetCursor(LOLGameConfig.Instance.DefaultCursor);
        }
    }

    private bool IsEnemyCamp(Camp targetCamp)
    {
        // 判断目标阵营是否为敌对
        // 这里假设Blue和Red互为敌对，Neutral为中立
        if (Camp.Type == CampType.Blue && targetCamp.Type == CampType.Red)
            return true;
        if (Camp.Type == CampType.Red && targetCamp.Type == CampType.Blue)
            return true;

        return false;
    }

    private void SetCursor(Texture2D cursorTexture)
    {
        if (cursorTexture != null && LOLGameConfig.Instance != null)
        {
            Cursor.SetCursor(cursorTexture, LOLGameConfig.Instance.CursorHotspot, CursorMode.Auto);
        }
        else
        {
            // 如果没有指定的光标纹理，恢复默认系统光标
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

}
