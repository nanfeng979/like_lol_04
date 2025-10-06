using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonUtils : MonoBehaviour
{
    /// <summary>
    /// 加载 JSON 并转为指定数据结构
    /// </summary>
    public static void LoadJson<T>(string key, Action<T> onComplete) where T : new()
    {
        AddressablesUtils.LoadAsset<TextAsset>(key, jsonAsset =>
        {
            if (jsonAsset == null)
            {
                onComplete?.Invoke(new T());
                return;
            }

            try
            {
                T data = JsonConvert.DeserializeObject<T>(jsonAsset.text);
                onComplete?.Invoke(data);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AddressablesUtils] JSON 解析失败: {e.Message}");
                onComplete?.Invoke(new T());
            }
        });
    }
}
