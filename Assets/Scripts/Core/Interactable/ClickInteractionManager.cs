using UnityEngine;

namespace LikeLoL04
{
    /// <summary>
    /// 处理玩家点击交互的管理器
    /// </summary>


    public class ClickInteractionManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask interactableLayerMask;

        public LOLGameObject player;

        void Awake()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1)) // 右键点击
            {
                HandleRightClick();
            }
        }

        private void HandleRightClick()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 10000f, interactableLayerMask))
            {
                Ground ground = hit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    Debug.Log("点击地面，移动到位置: " + hit.point);
                    player.MoveToPosition(hit.point);
                    return;
                }
                LOLGameObject lolGameObject = hit.collider.GetComponent<LOLGameObject>();
                if (lolGameObject != null)
                {
                    Debug.Log("点击可交互对象: " + lolGameObject.Name);
                    player.MoveToTarget(lolGameObject);
                    return;
                }
            }
        }
    }
}
