using System.Collections.Generic;
using LikeLoL04.EventSystem;
using UnityEngine;

namespace LikeLoL04
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class LOLGameObject : LOLObject
    {
        [SerializeField]
        public Camp Camp;

        public Animator animator { get; private set; }

        /// <summary>
        /// 状态机
        /// </summary>
        protected StateMachineV2 stateMachine;
        public StateMachineV2 StateMachine => stateMachine;

        public string DefaultStateId = "DefaultState";
        public string MoveStateId = "MoveState";
        public string AttackStateId = "AttackState";

        /// <summary>
        /// Buff 管理器
        /// </summary>
        private BuffManager buffManager;
        public BuffManager BuffManager => buffManager;
        // 兼容旧代码：只读暴露 BuffList（内部来自 BuffManager）
        public IReadOnlyList<Buff> BuffList => buffManager?.Buffs;

        /// <summary>
        /// 目标
        /// </summary>
        public LOLGameObject Target { get; set; }

        /// <summary>
        /// 鼠标当前悬停的单位（无需点击）。由交互管理器实时更新。
        /// </summary>
        public LOLGameObject HoverTarget { get; private set; }

        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 TargetPosition { get; set; }

        public bool IsMoveToTarget => Target != null;

        public float MoveSpeed { get; set; } = 200.0f;

        public float RotationDuration { get; set; } = 0.3f;

        [Header("Combat Settings")]
        [SerializeField]
        public float AttackRange = 200f;

        protected override void Start()
        {
            base.Start();
            animator = GetComponent<Animator>();

            stateMachine = new StateMachineV2();
            buffManager = new BuffManager(this);
            RegisterStates();

            // 初始状态设为待机
            SetDefaultState();

            // 配置 Attack -> Default 的过渡时长（未配置的其他过渡将使用状态机默认值）
            stateMachine.SetTransitionDuration("AttackState", "DefaultState", 0.12f);
        }

        protected override void Update()
        {
            base.Update();

            stateMachine.Update();

            buffManager.Update(Time.deltaTime);
        }

        protected virtual void RegisterStates()
        {
            // 使用字符串ID注册状态
            var defaultState = new LikeLoL04.DefaultStateV2(stateMachine, this);
            stateMachine.RegisterState("DefaultState", defaultState);
        }

        protected virtual void SetDefaultState()
        {
            stateMachine.TransitionTo(DefaultStateId);
        }

        // 移动速度（单位：单位/秒）
        private float moveSpeed = 200f;

        // 处理移动，返回是否到达停止距离
        public bool HandleMoveToPosition()
        {
            Vector3 currentPos = transform.position;
            float distance = Vector3.Distance(currentPos, TargetPosition);
            if (distance <= 0.5f)
            {
                return true; // 已到达
            }

            Vector3 direction = (TargetPosition - currentPos).normalized;
            Vector3 move = direction * moveSpeed * Time.deltaTime;
            if (move.magnitude > distance)
            {
                move = direction * distance;
            }
            transform.position = currentPos + move;
            return false;
        }

        public virtual void InteractWithTarget(LOLGameObject target)
        {
            Target = target;
            TargetPosition = target.transform.position;

            if (stateMachine.CurrentStateId == DefaultStateId)
            {
                stateMachine.TransitionTo(MoveStateId);
            }
        }

        public virtual void InteractWithPosition(Vector3 targetPos)
        {
            Target = null;
            TargetPosition = targetPos;

            if (stateMachine.CurrentStateId == DefaultStateId || stateMachine.CurrentStateId == AttackStateId)
            {
                stateMachine.TransitionTo(MoveStateId);
            }
        }

        // 普通平滑旋转：将前向 Z 轴朝向 targetPos（按固定角速度）
        public void HandleRotation(Vector3 targetPos)
        {
            // 仅在水平面旋转（忽略高度差），更符合大多数 MOBA/ARPG 相机与角色设定
            Vector3 lookDir = targetPos - transform.position;
            lookDir.y = 0f;

            // 目标方向过小则不旋转
            if (lookDir.sqrMagnitude <= 1e-6f)
                return;

            Quaternion desired = Quaternion.LookRotation(lookDir, Vector3.up);

            // 小角度直接对齐，避免抖动
            float angle = Quaternion.Angle(transform.rotation, desired);
            if (angle <= 0.1f)
            {
                transform.rotation = desired;
                return;
            }

            // 使用固定角速度的方式进行旋转（度/秒）。
            // 这里将 RotationDuration 解释为旋转 360° 约需要的时间，以获得直观可控的速度：
            float degreesPerSecond = 360f / Mathf.Max(0.0001f, RotationDuration);
            float maxStep = degreesPerSecond * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desired, maxStep);
        }

        // 目标在攻击范围内
        public bool IsTargetInAttackRange(LOLGameObject target)
        {
            if (target == null) return false;
            float distToTarget = Vector3.Distance(transform.position, target.transform.position);
            return distToTarget <= AttackRange;
        }

        public void AttackTargetListener()
        {
            // 若存在 Target 并且进入攻击范围，切换到 AttackState
            if (IsTargetInAttackRange(Target))
            {
                if (stateMachine.CurrentStateId == AttackStateId)
                    return;

                stateMachine.TransitionTo(AttackStateId);
                return;
            }
        }

        public void BeAttack()
        {
            // stateMachine.TransitionTo("BeAttack");
            Data.CurrentHp -= 50;
        }

        #region Hover Target Helpers
        /// <summary>
        /// 设置当前悬停目标（不与自己或重复对象进行多余操作）。
        /// 可在此加入高亮/描边等表现。
        /// </summary>
        public void SetHoverTarget(LOLGameObject newHover)
        {
            if (newHover == null || newHover == this)
                return;
            if (HoverTarget == newHover)
                return; // 未变化

            // 如需取消旧高亮，可在这里处理 HoverTarget
            HoverTarget = newHover;
        }

        /// <summary>
        /// 清除当前悬停目标。
        /// </summary>
        public void ClearHoverTarget()
        {
            if (HoverTarget == null) return;
            // TODO: Remove highlight logic from HoverTarget
            HoverTarget = null;

        }
        #endregion

        protected LOLGameObject _target { get => Target ?? HoverTarget; }

        // ==== BuffManager 便捷封装 ==== //
        public T AddBuff<T>(T buff) where T : Buff => buffManager.Add(buff);
        public T AddBuff<T>() where T : Buff => buffManager.Add<T>();
        public bool RemoveBuff<T>() where T : Buff => buffManager.Remove<T>();
        public bool HasBuff<T>() where T : Buff => buffManager.Has<T>();
        public T GetBuff<T>() where T : Buff => buffManager.Get<T>();
        public void ClearBuffs() => buffManager.Clear();

        public LOLGameObjectData Data;

    }

    [System.Serializable]
    public class LOLGameObjectData
    {

        private string _name = "LOLGameObject";
        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        private int _currentHp;

        public int CurrentHp
        {
            get => _currentHp;
            set
            {
                _currentHp = Mathf.Clamp(value, 0, MaxHp);
                EventBus.Emit("HealthChanged", Name, _currentHp, MaxHp);
            }
        }

        [SerializeField]
        private int _maxHp = 1000;

        public int MaxHp
        {
            get => _maxHp;
            set
            {
                _maxHp = Mathf.Max(1, value);
                if (_currentHp > _maxHp)
                    _currentHp = _maxHp;
            }
        }

        public Sprite Avatar;

        public int AttackValue;
    }
}
