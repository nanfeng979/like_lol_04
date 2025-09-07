# EventSystem 进度记录

日期：2025-09-07

内容：
- 新增 `EventBus`（On/Off/Once/Emit，泛型负载，同步分发）
- 新增 `EventSystemBoot`（DefaultExecutionOrder=-10000，BeforeSceneLoad 自动引导，可持久化）
- 新增 `README.md`（使用说明、扩展点）

下一步：
- 可选：提供 Editor 示例窗口，便于在运行时观察订阅/触发情况
- 可选：增加弱引用订阅或目标失效安全检查
- 可选：预留 IEvent 接口与命名式频道（string/Enum）并提供双路由
