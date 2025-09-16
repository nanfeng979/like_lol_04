
using UnityEngine;

namespace LikeLoL04
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class LOLGameObject : LOLObject
    {
        [SerializeField]
        public Camp Camp;

        public Animator Animator { get; private set; }

        /// <summary>
        /// 状态机
        /// </summary>
        protected StateMachine stateMachine;

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

        public float RotationDuration { get; set; } = 0.1f;

        [Header("Combat Settings")]
        [SerializeField]
        public float AttackRange = 200f;

        protected override void Start()
        {
            base.Start();
            Animator = GetComponent<Animator>();

            stateMachine = new StateMachine();
            stateMachine.RegisterState(new DefaultState(stateMachine, this));

            // 初始状态设为待机
            SetDefaultState();

            // 配置 Attack -> Default 的过渡时长（未配置的其他过渡将使用状态机默认值）
            stateMachine.SetTransitionDuration<AttackState, DefaultState>(0.12f);
        }

        protected override void Update()
        {
            base.Update();

            stateMachine.Update();
        }

        public void MoveToPosition(Vector3 position)
        {
            // 移动到指定位置：如果当前已处于移动状态，仅更新目标位置，避免重复进入导致动画重置
            Target = null; // 清空目标对象，确保以目标点为准
            TargetPosition = position;

            if (stateMachine.CurrentStateType == typeof(MoveState))
            {
                return;
            }

            stateMachine.TransitionTo<MoveState>();
        }

        public void MoveToTarget(LOLGameObject target)
        {
            // 移动到指定目标
            Target = target;
            TargetPosition = Vector3.zero; // 清空目标位置，确保以目标对象为准

            if (stateMachine.CurrentStateType == typeof(MoveState))
            {
                return;
            }

            stateMachine.TransitionTo<MoveState>();
        }

        protected virtual void SetDefaultState()
        {
            stateMachine.TransitionTo<DefaultState>();
        }
    }
}
