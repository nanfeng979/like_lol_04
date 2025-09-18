using UnityEngine;
namespace LikeLoL04
{
    public class LOLMuZhuangGameObject : LOLGameObject
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void RegisterStates()
        {
            base.RegisterStates();
            stateMachine.RegisterState(new BeAttack(stateMachine, this));
        }

        public override void InteractWithTarget(LOLGameObject target)
        {
        }

        public override void InteractWithPosition(Vector3 targetPos)
        {
        }

    }
}
