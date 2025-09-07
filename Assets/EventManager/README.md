# 事件系统（MVP）

本模块提供一个全局事件总线，用于在系统内分发自定义事件。当前为 MVP 版本：
- API：On / Off / Once / Emit（泛型参数）
- 分发：同步
- 不包含：队列、缓存、优先级、线程安全、多场景隔离

## 主要文件
- `Scripts/Runtime/EventBus.cs`：静态事件总线，核心逻辑。
- `Scripts/Runtime/EventSystemBoot.cs`：启动器，确保事件系统在场景加载前初始化。

## 使用

```csharp
using LikeLoL04.EventSystem;

// 1. 定义事件负载类型（推荐使用结构体或只读类）
public struct PlayerLevelUp
{
    public int playerId;
    public int newLevel;
    public PlayerLevelUp(int playerId, int newLevel)
    {
        this.playerId = playerId;
        this.newLevel = newLevel;
    }
}

// 2. 订阅
EventBus.On<PlayerLevelUp>(e => UnityEngine.Debug.Log($"LevelUp: {e.playerId}->{e.newLevel}"));

// 3. 一次性订阅
EventBus.Once<PlayerLevelUp>(e => UnityEngine.Debug.Log($"Once LevelUp: {e.newLevel}"));

// 4. 分发
EventBus.Emit(new PlayerLevelUp(1, 2));

// 5. 取消订阅
void OnDisable()
{
    EventBus.Off<PlayerLevelUp>(OnLevelUp);
}
void OnLevelUp(PlayerLevelUp e) { /* ... */ }
```

## 约定
- 以事件负载类型 `T` 作为“频道键”，相同 `T` 的订阅者共享分发。
- `Once` 内部通过包装委托实现，支持 `Off(原始回调)` 正确移除。
- 分发使用快照，允许回调内 `Off/Once` 修改订阅而不抛异常。

## 扩展点（后续）
- 异步/队列/节流/防抖
- 事件优先级与分组
- 编辑器监控与统计
- 线程安全与跨线程派发

```
执行顺序
- 通过 `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)` + `DefaultExecutionOrder(-10000)` + `Awake()` 确保尽早初始化
```
