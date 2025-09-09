# 新动作系统文档

## 系统概述

新动作系统是一个基于状态模式和命令模式的游戏角色控制系统，专为类似英雄联盟的游戏设计。系统具有高度的模块化和可扩展性。

## 核心设计模式

### 1. 状态模式 (State Pattern)
- **IState**: 状态接口，定义状态的基本行为
- **BaseState**: 状态基类，提供通用实现
- **具体状态**: IdleState（待机）、MoveState（移动）、AttackState（攻击）

### 2. 命令模式 (Command Pattern)
- **ICommand**: 命令接口，定义命令的基本行为
- **CommandInvoker**: 命令调用器，管理命令的执行和撤销
- **具体命令**: MoveCommand（移动）、AttackCommand（攻击）、StopCommand（停止）

## 系统架构

```
NewActionSystem/
├── Core/                    # 核心接口和类
│   ├── IState.cs           # 状态接口
│   ├── ICommand.cs         # 命令接口
│   ├── StateMachine.cs     # 状态机管理器
│   └── CommandInvoker.cs   # 命令调用器
├── States/                 # 状态实现
│   ├── BaseState.cs        # 状态基类
│   ├── IdleState.cs        # 待机状态
│   ├── MoveState.cs        # 移动状态
│   └── AttackState.cs      # 攻击状态
├── Commands/               # 命令实现
│   ├── MoveCommand.cs      # 移动命令
│   ├── AttackCommand.cs    # 攻击命令
│   └── StopCommand.cs      # 停止命令
└── Player/                 # 玩家相关
    └── PlayerController.cs # 玩家控制器
```

## 状态说明

### IdleState（待机状态）
- **功能**: 玩家站立不动的默认状态
- **动画**: 播放"Idle"动画
- **转换条件**: 
  - 有移动输入 → MoveState
  - 有攻击输入 → AttackState

### MoveState（移动状态）
- **功能**: 玩家移动时的状态
- **动画**: 播放"Move"动画
- **转换条件**:
  - 无移动输入 → IdleState
  - 有攻击输入 → AttackState

### AttackState（攻击状态）
- **功能**: 玩家攻击时的状态
- **动画**: 播放"Attack"动画
- **特性**: 
  - 攻击期间停止移动
  - 攻击动画中段造成伤害
  - 攻击结束后根据输入切换状态

## 命令说明

### MoveCommand（移动命令）
- **功能**: 控制玩家移动到指定位置
- **支持撤销**: 是

### AttackCommand（攻击命令）
- **功能**: 控制玩家攻击指定目标
- **支持撤销**: 是

### StopCommand（停止命令）
- **功能**: 停止所有动作
- **支持撤销**: 否

## 输入控制

- **WASD**: 键盘移动
- **右键**: 点击移动到指定位置
- **左键**: 点击攻击目标
- **S键**: 停止所有动作

## 扩展指南

### 添加新状态
1. 创建继承自`BaseState`的新状态类
2. 实现`OnEnter`、`OnUpdate`、`OnExit`方法
3. 在`PlayerController`中注册新状态

### 添加新命令
1. 创建实现`ICommand`接口的新命令类
2. 实现`Execute`、`Undo`、`CanExecute`方法
3. 在适当的地方使用`CommandInvoker`执行命令

## 注意事项

1. 所有状态切换都有验证机制，确保状态转换的合法性
2. 命令系统支持撤销功能，便于实现复杂的游戏机制
3. 动画触发需要在Animator中配置对应的Trigger参数
4. 系统设计为单例模式，适合单人游戏场景

## 依赖组件

PlayerController需要以下Unity组件：
- **Animator**: 用于播放动画
- **Rigidbody**: 用于物理移动
- **Collider**: 用于碰撞检测（可选）
