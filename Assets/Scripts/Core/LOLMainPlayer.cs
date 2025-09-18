using LikeLoL04.EventSystem;
using UnityEngine;

namespace LikeLoL04
{
    public class LOLMainPlayer : LOLGameObject
    {

        protected override void Start()
        {
            base.Start();
            Camp = new Camp { Type = CampType.Blue };

            AttackRange = 200f;
        }

        protected override void Update()
        {
            base.Update();

            // 按下 Q 键切换到 Spell1 状态
            if (Input.GetKeyDown(KeyCode.Q))
            {
                stateMachine.TransitionTo<Spell1_default>();
                useQSkill = true;
                qSkillTimer = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                stateMachine.TransitionTo<Spell3>();
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
        }

        protected override void RegisterStates()
        {
            base.RegisterStates();
            stateMachine.RegisterState(new MoveState(stateMachine, this));
            stateMachine.RegisterState(new AttackState(stateMachine, this));
            stateMachine.RegisterState(new Spell1_default(stateMachine, this));
            stateMachine.RegisterState(new Spell1_attack(stateMachine, this));
            stateMachine.RegisterState(new Spell1_run(stateMachine, this));
            stateMachine.RegisterState(new Spell3(stateMachine, this));
        }

        public bool useQSkill = false;
        public float qSkillTimer = 0f;
        public float qSkillDuration = 10f;

        public override void InteractWithTarget(LOLGameObject target)
        {
            Target = target;
            TargetPosition = target.transform.position;

            if (stateMachine.CurrentStateType == typeof(Spell1_default) || stateMachine.CurrentStateType == typeof(Spell1_run))
            {
                MoveToPosition(TargetPosition);
                stateMachine.TransitionTo<Spell1_attack>();
            }

            if (stateMachine.CurrentStateType == typeof(DefaultState))
            {
                MoveToPosition(TargetPosition);
            }
        }

        public override void InteractWithPosition(Vector3 targetPos)
        {
            Target = null;
            TargetPosition = targetPos;

            if (stateMachine.CurrentStateType == typeof(Spell1_default))
            {
                stateMachine.TransitionTo<Spell1_run>();
                return;
            }

            if (stateMachine.CurrentStateType == typeof(DefaultState))
            {
                MoveToPosition(targetPos);
            }
        }
    }
}

