using System;
using UnityEngine;
namespace LikeLoL04
{
    public class LOLGailLunGameObject : LOLGameObject
    {
        private bool useQSkill = false;
        private float qSkillTimer = 0f;
        private float qSkillDuration = 10f;

        protected override void Start()
        {
            base.Start();

            DefaultStateId = "DefaultState";
            MoveStateId = "MoveState";

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
                // stateMachine.TransitionTo("Spell1_default");
                // useQSkill = true;
                // qSkillTimer = 0f;
                BuffList.Add(new GailunQBuff(this));
                BuffList[0].OnApply();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                stateMachine.TransitionTo("Spell3");
            }

            // 处理 Q 技能持续时间
            if (useQSkill)
            {
                qSkillTimer += Time.deltaTime;
                if (qSkillTimer >= qSkillDuration)
                {
                    useQSkill = false;
                    qSkillTimer = 0f;
                }
            }

            if (stateMachine.CurrentStateId == "MoveState" && Target != null)
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
            // stateMachine.RegisterState(new Spell1_default(stateMachine, this));
            // stateMachine.RegisterState(new Spell1_attack(stateMachine, this));
            stateMachine.RegisterState("Spell1_run", new Spell1_runV2(stateMachine, this));
            // stateMachine.RegisterState(new Spell3(stateMachine, this));
        }

        public override void InteractWithTarget(LOLGameObject target)
        {
            Target = target;
            TargetPosition = target.transform.position;

            // if (stateMachine.CurrentStateId == "Spell1_default" || stateMachine.CurrentStateId == "Spell1_run")
            // {
            //     stateMachine.TransitionTo("Spell1_attack");
            // }

            if (stateMachine.CurrentStateId == "DefaultState")
            {
                stateMachine.TransitionTo("MoveState");
            }
        }

        public override void InteractWithPosition(Vector3 targetPos)
        {
            Target = null;
            TargetPosition = targetPos;

            // if (stateMachine.CurrentStateType == typeof(Spell1_default))
            // {
            //     stateMachine.TransitionTo<Spell1_run>();
            //     return;
            // }

            if (stateMachine.CurrentStateId == DefaultStateId)
            {
                Debug.Log("Transition to MoveState : " + MoveStateId);
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
