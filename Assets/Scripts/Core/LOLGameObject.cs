
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
        private StateMachine stateMachine;

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

    }
}
