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
            stateMachine.RegisterState(new MoveState(stateMachine, this));
            stateMachine.RegisterState(new AttackState(stateMachine, this));

            stateMachine.TransitionTo<AttackState>();
        }

        protected override void Update()
        {
            base.Update();
        }

    }
}

