using System;

namespace LikeLoL04
{
    /// <summary>
    /// 移动状态
    /// </summary>
    public class Spell1_runV2 : BaseStateV2
    {
        #region Constructor

        public Spell1_runV2(StateMachineV2 stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
        }

        #endregion

        #region State Implementation

        public override void OnEnter()
        {
            base.OnEnter();
            float dur = stateMachine.CurrentTransitionDuration;
            selfLOLGameObject.animator.CrossFade("Spell1_run", dur, -1, 0f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (selfLOLGameObject.HandleMoveToPosition())
            {
                stateMachine.TransitionTo(selfLOLGameObject.DefaultStateId);
                return;
            }

            // 处理旋转
            selfLOLGameObject.HandleRotation(selfLOLGameObject.TargetPosition);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override bool CanTransitionTo(Type targetState)
        {
            return targetState != typeof(MoveState);
        }

        #endregion

    }

}
