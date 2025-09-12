
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

        public float MoveSpeed { get; set; } = 200.0f;

        public float RotationDuration { get; set; } = 0.1f;

        protected override void Start()
        {
            base.Start();
            Animator = GetComponent<Animator>();

            stateMachine = new StateMachine();
            stateMachine.RegisterState(new DefaultState(stateMachine, this));

            // 初始状态设为待机
            stateMachine.TransitionTo<DefaultState>();
        }

        protected override void Update()
        {
            base.Update();

            stateMachine.Update();
        }

        public void MoveToPosition(Vector3 position)
        {
            TargetPosition = position;
            stateMachine.TransitionTo<MoveState>();
        }
    }
}
