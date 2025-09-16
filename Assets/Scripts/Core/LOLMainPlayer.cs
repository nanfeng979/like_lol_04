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
                stateMachine.TransitionTo<Spell1_run>();
            }
        }

        protected override void RegisterStates()
        {
            base.RegisterStates();
            stateMachine.RegisterState(new MoveState(stateMachine, this));
            stateMachine.RegisterState(new AttackState(stateMachine, this));
            stateMachine.RegisterState(new Spell1(stateMachine, this));
            stateMachine.RegisterState(new Spell1_run(stateMachine, this));
        }
    }
}

