
using System;
using System.Collections.Generic;
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
        /// 目标
        /// </summary>
        public LOLGameObject Target { get; set; }

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

        public List<Buff> BuffList { get; private set; } = new List<Buff>();

        protected override void Start()
        {
            base.Start();
            animator = GetComponent<Animator>();

            stateMachine = new StateMachineV2();
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

            for (int i = 0; i < BuffList.Count; i++)
            {
                Buff buff = BuffList[i];
                buff.OnUpdate(Time.deltaTime);

                if (buff.IsExpired())
                {
                    buff.OnRemove();
                    BuffList.RemoveAt(i);
                    i--;
                }
            }
        }

        protected virtual void RegisterStates()
        {
            // 使用字符串ID注册状态
            var defaultState = new LikeLoL04.DefaultStateV2(stateMachine, this);
            stateMachine.RegisterState("DefaultState", defaultState);
            
            // 暂时只注册默认状态，其他状态可以后续添加
            // var moveState = new MoveStateV2(stateMachine, this);
            // stateMachine.RegisterState("MoveState", moveState);
            // var attackState = new AttackStateV2(stateMachine, this);
            // stateMachine.RegisterState("AttackState", attackState);
            // var attack2State = new Attack2StateV2(stateMachine, this);
            // stateMachine.RegisterState("Attack2State", attack2State);
            // var beAttackState = new BeAttackV2(stateMachine, this);
            // stateMachine.RegisterState("BeAttack", beAttackState);
        }

        protected virtual void SetDefaultState()
        {
            stateMachine.TransitionTo("DefaultState");
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

            if (stateMachine.CurrentStateId == "DefaultState")
            {
                stateMachine.TransitionTo("MoveState");
            }
        }

        public virtual void InteractWithPosition(Vector3 targetPos)
        {
            Target = null;
            TargetPosition = targetPos;

            if (stateMachine.CurrentStateId == "DefaultState")
            {
                stateMachine.TransitionTo("MoveState");
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
                stateMachine.TransitionTo(AttackStateId);
                return;
            }
        }

        public void BeAttack()
        {
            stateMachine.TransitionTo("BeAttack");
        }

    }
}
