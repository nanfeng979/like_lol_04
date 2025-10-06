using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressablesUtils
{
    // 缓存已加载资源，避免重复加载
    private static readonly Dictionary<string, UnityEngine.Object> _cache = new();

    /// <summary>
    /// 加载Addressables资源（带缓存）
    /// </summary>
    public static void LoadAsset<T>(string key, Action<T> onComplete) where T : UnityEngine.Object
    {
        // 如果缓存中已有，直接返回
        if (_cache.TryGetValue(key, out var cachedObj))
        {
            onComplete?.Invoke(cachedObj as T);
            return;
        }

        // 异步加载资源
        Addressables.LoadAssetAsync<T>(key).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _cache[key] = handle.Result;
                onComplete?.Invoke(handle.Result);
            }
            else
            {
                Debug.LogError($"[AddressablesUtils] 资源加载失败: {key}");
                onComplete?.Invoke(null);
            }
        };
    }

    /// <summary>
    /// 释放某个资源
    /// </summary>
    public static void Release(string key)
    {
        if (_cache.TryGetValue(key, out var obj))
        {
            Addressables.Release(obj);
            _cache.Remove(key);
        }
    }

    /// <summary>
    /// 清理所有缓存资源
    /// </summary>
    public static void ClearCache()
    {
        foreach (var kv in _cache)
        {
            Addressables.Release(kv.Value);
        }
        _cache.Clear();
    }
}
