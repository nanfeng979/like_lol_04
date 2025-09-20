using System;

namespace LikeLoL04
{
    /// <summary>
    /// 移动状态V2，适配StateMachineV2
    /// </summary>
    public class MoveStateV2 : BaseStateV2
    {

        #region Constructor

        public MoveStateV2(StateMachineV2 stateMachine, LOLGameObject LOLGameObject)
            : base(stateMachine, LOLGameObject)
        {
        }

        #endregion

        #region State Implementation

        public override void OnEnter()
        {
            base.OnEnter();
            if (selfLOLGameObject.animator != null)
            {
                float dur = stateMachine.CurrentTransitionDuration;
                selfLOLGameObject.animator.CrossFade("Move", dur, -1, 0f);
            }
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
            return targetState.Name != "MoveStateV2";
        }

        #endregion

    }

}