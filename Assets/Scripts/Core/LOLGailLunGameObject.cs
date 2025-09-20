using System;
using UnityEngine;
namespace LikeLoL04
{
    public class LOLGailLunGameObject : LOLGameObject
    {
        protected override void Start()
        {
            base.Start();

            DefaultStateId = "DefaultState";
            MoveStateId = "MoveState";
            AttackStateId = "AttackState";

            AttackRange = 200f;
            stateMachine.SetTransitionDuration("Attack2State", "DefaultState", 0.08f);
            stateMachine.SetTransitionDuration("AttackState", "Attack2State", 0.08f);
        }

        protected override void Update()
        {
            base.Update();

            // 按下 Q 键切换到 Spell1 状态
            if (Input.GetKeyDown(KeyCode.Q))
            {
                BuffList.Add(new GailunQBuff(this));
                BuffList[0].OnApply();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                stateMachine.TransitionTo("Spell3");
            }

            if (stateMachine.CurrentStateId == MoveStateId && Target != null)
            {
                AttackTargetListener();
            }
        }

        protected override void RegisterStates()
        {
            base.RegisterStates();
            stateMachine.RegisterState("MoveState", new MoveStateV2(stateMachine, this));
            stateMachine.RegisterState("AttackState", new AttackStateV2(stateMachine, this));
            stateMachine.RegisterState("Attack2State", new Attack2StateV2(stateMachine, this));
            stateMachine.RegisterState("Spell1_attack", new Spell1_attackV2(stateMachine, this));
            stateMachine.RegisterState("Spell1_run", new Spell1_runV2(stateMachine, this));
        }

        public override void InteractWithTarget(LOLGameObject target)
        {
            Target = target;
            TargetPosition = target.transform.position;

            if (stateMachine.CurrentStateId == DefaultStateId)
            {
                stateMachine.TransitionTo(MoveStateId);
            }
        }

        public override void InteractWithPosition(Vector3 targetPos)
        {
            Target = null;
            TargetPosition = targetPos;

            if (stateMachine.CurrentStateId == DefaultStateId)
            {
                stateMachine.TransitionTo(MoveStateId);
            }
        }

        public void AttackTarget()
        {
            if (Target != null)
            {
                Target.BeAttack();
            }
        }
    }
}
