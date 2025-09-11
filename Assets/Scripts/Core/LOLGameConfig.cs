using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLGameConfig : MonoBehaviour
{
    public static LOLGameConfig Instance { get; private set; }

    [Header("鼠标光标配置")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D attackCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    public Texture2D DefaultCursor => defaultCursor;
    public Texture2D AttackCursor => attackCursor;
    public Vector2 CursorHotspot => cursorHotspot;

    void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCursorResources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void LoadCursorResources()
    {
        // 如果没有设置光标资源，尝试从Resources加载
        if (defaultCursor == null)
        {
            defaultCursor = Resources.Load<Texture2D>("Cursors/Cursor_Basic");
        }
        if (attackCursor == null)
        {
            attackCursor = Resources.Load<Texture2D>("Cursors/Cursor_Attack");
        }
    }
}
