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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                BuffManager.Add(new GailunQBuff(this));
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                stateMachine.TransitionTo("Spell3");
            }
            else if (Input.GetKeyDown(KeyCode.R) && _target != null && IsTargetInAttackRange(_target))
            {
                stateMachine.TransitionTo("Spell4");
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
            stateMachine.RegisterState("Spell3", new Spell3LuaState(stateMachine, this));
            stateMachine.RegisterState("Spell4", new Spell4LuaState(stateMachine, this));
        }

        public override void InteractWithTarget(LOLGameObject target)
        {
            base.InteractWithTarget(target);
        }

        public override void InteractWithPosition(Vector3 targetPos)
        {
            base.InteractWithPosition(targetPos);
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
