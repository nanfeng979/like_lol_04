using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Game.Bag.Model;
using UnityEngine;
using XLua;

public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance;

    private void Awake()
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

        // await LoadItemDataFromLuaAsync();
    }

    [Tooltip("StreamingAssets 下物品表 Lua 相对路径（优先级高于 JSON）")] public string itemsLuaPath = "Bag/items.lua";

    private ItemDatabase _itemDatabase = new ItemDatabase();

    /// <summary>
    /// 异步获取物品数据库（确保已加载）
    /// </summary>
    public async Task<ItemDatabase> GetItemDatabaseAsync()
    {
        if (_itemDatabase == null || _itemDatabase.items == null || _itemDatabase.items.Count == 0)
        {
            var db = await LoadItemDataFromLuaAsync();
            if (db != null)
            {
                _itemDatabase = db;
            }
        }
        return _itemDatabase.BuildIndex();
    }

    private LuaEnv _luaEnv; // 持久化的 Lua 环境，供 usedAction 使用

    /// <summary>
    /// 通过 XLua 读取 items.lua（返回一个 Lua 表或模块），并转换为 ItemDatabase。
    /// </summary>
    private async Task<ItemDatabase> LoadItemDataFromLuaAsync()
    {
        try
        {
            var path = Path.Combine(Application.streamingAssetsPath, itemsLuaPath);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"BagController：未找到 items.lua：{path}");
                return null;
            }

            string luaCode = await ReadAllTextAsync(path);
            // 复用同一 LuaEnv，便于 usedAction 持续可用
            if (_luaEnv == null) _luaEnv = new LuaEnv();
            // 向 Lua 注入 C# EventBus，便于在 usedAction 内直接调用
            _luaEnv.Global.Set("print", new Action<string>(Debug.Log));
            _luaEnv.Global.Set("EventBus", typeof(LikeLoL04.EventSystem.EventBus));

            object[] ret = _luaEnv.DoString(luaCode, "items.lua");
            if (ret == null || ret.Length == 0)
            {
                Debug.LogWarning("BagController：items.lua 未返回任何值（需要 return 一个表）");
                return null;
            }

            var root = ret[0] as LuaTable;
            if (root == null)
            {
                Debug.LogWarning("BagController：items.lua 返回的不是表");
                return null;
            }

            var db = new ItemDatabase();
            var itemsTbl = root.Get<LuaTable>("items");
            if (itemsTbl == null)
            {
                Debug.LogWarning("BagController：items.lua 中缺少 items 表");
                return db;
            }

            var list = new List<Game.Bag.Model.ItemData>();
            itemsTbl.ForEach<int, LuaTable>((idx, itemTbl) =>
            {
                if (itemTbl == null) return;
                var data = new Game.Bag.Model.ItemData();
                data.itemId = itemTbl.Get<string>("itemId");
                if (string.IsNullOrEmpty(data.itemId)) return;
                data.displayName = itemTbl.Get<string>("displayName");
                data.iconAddress = itemTbl.Get<string>("iconAddress");
                int ms = 0;
                try { ms = itemTbl.Get<int>("maxStack"); } catch { ms = 0; }
                data.maxStack = ms > 0 ? ms : 99;

                // usedAction: 直接是 Lua 函数（可选）
                try
                {
                    var used = itemTbl.Get<System.Action>("usedAction");
                    data.usedAction = used; // 绑定到 ItemData，以便右键直接调用
                }
                catch { /* 忽略取值失败 */ }

                list.Add(data);
            });

            db.items = list;
            return db;
        }
        catch (Exception ex)
        {
            Debug.LogError($"LoadItemDataFromLuaAsync 错误：{ex}");
            return null;
        }

    }

    private static async Task<string> ReadAllTextAsync(string path)
    {
        using (var reader = File.OpenText(path))
        {
            return await reader.ReadToEndAsync();
        }
    }

    private void OnDestroy()
    {
        if (_luaEnv != null)
        {
            try
            {
                _luaEnv.Dispose();
                _luaEnv = null;
            }
            catch { };
        }
    }
}
