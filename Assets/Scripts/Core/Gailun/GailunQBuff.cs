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

            owner.StateMachine.RegisterState("Spell1_run", new Spell1_runV2(owner.StateMachine, owner));
            owner.MoveStateId = "Spell1_run";
            owner.StateMachine.RegisterState("Spell1_attack", new Spell1_attackV2(owner.StateMachine, owner));
            owner.AttackStateId = "Spell1_attack";
            
            if (owner.StateMachine.CurrentStateId == "MoveState")
            {
                owner.StateMachine.TransitionTo(owner.MoveStateId);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            owner.MoveStateId = "MoveState";
            owner.AttackStateId = "AttackState";
            if (owner.StateMachine.CurrentStateId == "Spell1_run")
            {
                owner.StateMachine.TransitionTo(owner.MoveStateId);
            }
        }
    }
}
