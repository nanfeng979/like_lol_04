namespace LikeLoL04
{
    /// <summary>
    /// 盖伦Q技能的Buff
    /// </summary>
    public class GailunQBuff : Buff
    {
        public GailunQBuff(LOLGameObject owner) : base(owner)
        {
            islimited = true;
            duration = 3f;
        }

        public override void OnApply()
        {
            base.OnApply();
            owner.MoveStateId = "Spell1_run";
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            owner.MoveStateId = "MoveState";
            owner.StateMachine.TransitionTo(owner.MoveStateId);
        }
    }
}
