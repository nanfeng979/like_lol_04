# Bag System - Item Actions（MVP）

本模块支持在 `items.json` 中为 Item 配置右键行为。首版仅实现 `emit` 动作：在右键点击时通过 `EventBus` 同步分发一个命名事件。

## JSON 示例

使用 Unity JsonUtility 友好结构：
```json
{
  "items": [
    {
      "itemId": "potion",
      "displayName": "生命药水",
      "iconAddress": "Icons/potion.png",
      "maxStack": 99,
      "actions": [
        { "id": "emit", "label": "测试事件", "eventName": "TestEvent", "args": ["a", "b", "c"] }
      ]
    }
  ]
}
```

## 运行时行为
- 在 `BagSlotView` 中实现了 `IPointerClickHandler`，右键时：
  - 查询槽位对应 Item 的 `actions`，找到第一个 `id=="emit"` 的动作
  - 调用 `BagController.ExecuteAction()`，内部使用 `EventBus.Emit(eventName, args)` 分发

## 订阅示例
```csharp
using LikeLoL04.EventSystem;

void OnEnable()
{
    EventBus.On("TestEvent", OnTestEvent);
}

void OnDisable()
{
    EventBus.Off("TestEvent", OnTestEvent);
}

private void OnTestEvent(object[] args)
{
    UnityEngine.Debug.Log($"TestEvent received: {string.Join(",", args ?? System.Array.Empty<object>())}");
}
```

## 备注
- 仅同步分发；无队列/缓存。
- 若后续扩展更多动作，可在 `BagController.ExecuteAction` 内按 `action.id` 增加分支或引入动作注册表。
