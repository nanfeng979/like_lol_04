# 背包系统（MVP）需求说明

更新时间：2025-09-04

## 概述
- 目标：先实现一个“简单、可渲染”的背包系统，仅负责把 JSON 数据按网格渲染到 UGUI。先不做交互与复杂逻辑。
- 风格：整体 UI 与装备视觉可参考英雄联盟风格（深色底、金色描边/数值、微高光与内阴影），字体使用 Unity 默认字体（不内置付费字体）。
- 技术栈：仅使用 UGUI；资源加载使用 Addressables（不使用 Resources）。
- 架构：MVC。Model（纯 C#，非 MonoBehaviour） ←→ Controller ←→ View（UGUI，MonoBehaviour）。所有流转由 Controller 统一调度管理。

## 最小可行范围（MVP）
- 网格布局：NxM（默认 4x5）。首版固定为 4x5，预留从 JSON 配置覆盖尺寸。
- 数据来源：StreamingAssets 内的 2 个 JSON 文件。
  - items.json：道具静态表（含 itemId、显示名、icon 的 Addressables Key、maxStack 等）。
  - bag.json：背包实例（含 grid 尺寸与条目列表，每条目含 itemId、quantity）。
- 渲染项：
  - 每个格子渲染：背景、图标（来自 Addressables）、数量（文本）。
  - 名字（displayName）暂不渲染，保留 Tooltip（未来项）。
- 叠堆：默认允许叠堆；缺省上限为 99，可被 items.json 的 maxStack 覆盖。
- 容量：当 bag.json 的条目超出容量（rows × columns）时，截断显示，并输出日志警告。
- 多语言：首版不做；在文档中标记为“未来扩展”。

## 数据契约（JSON 结构）
- StreamingAssets 目录建议：
  - StreamingAssets/Inventory/items.json
  - StreamingAssets/Inventory/bag.json

- items.json（示例）
```json
{
  "items": [
    {
      "itemId": "potion_small",
      "displayName": "小型生命药水",
      "iconAddress": "Inventory/Icons/potion_small", 
      "maxStack": 99
    },
    {
      "itemId": "sword_iron",
      "displayName": "铁剑",
      "iconAddress": "Inventory/Icons/sword_iron",
      "maxStack": 1
    }
  ]
}
```

- bag.json（示例：按 index 明确放置位置）
```json
{
  "grid": { "rows": 4, "columns": 5 },
  "items": [
    { "index": 0, "itemId": "potion_small", "quantity": 12 },
    { "index": 7, "itemId": "sword_iron", "quantity": 1 }
  ]
}
```

说明：
- grid.rows/columns 缺省时，按默认 4x5 处理。
- items 顺序即填充顺序（先行后列）。超容量条目忽略并告警。
- iconAddress 为 Addressables 的 Key（示例建议分组为 "Inventory/Icons/*"）。

## MVC 架构约定
- Model（纯数据，非 MonoBehaviour）：
  - ItemData：道具静态数据（itemId、displayName、iconAddress、maxStack）。
  - BagItem：背包中的一条目（itemId、quantity）。
  - InventorySnapshot：当前背包的整体数据（grid 尺寸、BagItem 集合）。
- View（UGUI，MonoBehaviour）：
  - InventoryView：承载 GridLayoutGroup 与 Slot 预制的容器。
  - InventorySlotView：单格子视图，包含：背景、Icon Image、数量 Text。
  - Tooltip（未来）：显示名字、属性等（暂不实现）。
- Controller：
  - InventoryController：
    - 负责读取 StreamingAssets JSON（items.json、bag.json）并做轻量校验。
    - 通过 Addressables 异步加载图标 Sprite。
    - 绑定数据到 View（生成/复用格子、设置 Icon 与数量）。
    - 统一管理 View 与 Model 的交互流向（Model ←→ Controller ←→ View）。

数据流：
- 初始化：Controller 读取 JSON → 组装 Model → Addressables 取图 → 渲染 View。
- 更新：运行时交换/移动仅更新内存快照；关闭背包时才写回 JSON（持久化）。

## 视觉与样式（LoL 风格参考）
- 调色建议：
  - 背景：#0A1428（深蓝黑）
  - 描边/金属：#C89B3C（金色）
  - 数量文本：金色/浅金色，描边或投影增强可读性
- 格子：
  - 金色描边、微内阴影；鼠标悬停高亮（未来可加）。
- 名字与属性：
  - 暂不渲染；未来在 Tooltip 中使用金色标题与有层次的说明项。
- 字体：
  - Unity 默认字体。后续如需替换，保证版权合规再更新。

## Addressables 约定
- Icon 资源需标记为 Addressable，Key 与 items.json 的 iconAddress 对应。
- 建议分组：Inventory/Icons/*（组名与打包策略可后续细化）。
- 首版允许在 Editor 下直接异步加载；构建时需确保该组被打进包。

## 错误处理与日志
- 未找到 itemId：跳过该条目并 Debug.LogWarning（含 itemId）。
- 未找到 iconAddress：使用占位图（内置一张问号/灰底图）并 Warning。
- 超容量：截断多余条目并 Warning（含溢出数量）。
- 数量校验：<1 记为 1；>maxStack 则截断为 maxStack 并 Warning（道具不可叠则强制为 1）。

## 非目标（本次不实现，留作未来）
- 名字渲染与 Tooltip（名字、品质、属性展示）。
- 拖拽交换、右键菜单、道具使用。
- 排序/筛选、分页、多背包联动。
- 存档落盘（写回）与版本兼容。
- 多语言与本地化（displayName 可存多语言 Key，后续接入）。
- Addressables 远端分发与热更新策略。

## 目录与资源组织（建议）
- Assets/Study/003-BagSystem/001-BagSystem/           （模块脚本与文档）
- Assets/AddressableAssets/Inventory/Icons/            （道具图标，Addressables）
- Assets/AddressableAssets/Inventory/Placeholders/     （占位图）
- Assets/Prefabs/Inventory/                             （UGUI 预制：InventoryView、Slot）
- StreamingAssets/Inventory/                            （items.json、bag.json）

## 验收标准（MVP）
- 在一个测试场景中，挂载 InventoryView（含 GridLayoutGroup）与 InventoryController。
- 运行后读取 StreamingAssets 的两份 JSON。
- 实际渲染出 4x5 网格；图标正确显示；数量文本正确叠加；名字不渲染。
- 超容量、缺图标、未知 itemId 等情况有 Warning 日志，并可见合理的占位显示。
- 整体观感符合 LoL 深色+金色主题基调。

## 未来扩展清单（将来实现）
- 配置化网格尺寸与自适应布局（横竖屏与缩放）。
- Tooltip（名字、品质、属性、来源描述）。
- 操作：拖拽、拆分/合堆、右键菜单、使用/丢弃。
- 筛选/排序、分页、多背包、多容器。
- 存档落盘（JSON/二进制/云存档），版本迁移。
- 本地化（多语言）、无障碍（色盲友好）。
- Addressables 分组与远端分发策略、缓存清理与诊断面板。

## 备注
- 名字渲染与 Tooltip：明确为“未来项”，首版不做。
- 仅使用 UGUI 与 Addressables；不使用 Resources。
- Model 为纯 C# 类型，不继承 MonoBehaviour；控制器统一驱动。