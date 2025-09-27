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
            stateMachine.SetTransitionDuration("Spell3", "DefaultState", 0f);
        }

        protected override void Update()
        {
            base.Update();

            if (stateMachine.CurrentStateId == MoveStateId && Target != null)
            {
                AttackTargetListener();
            }
        }

        void OnEnable()
        {
            TrySubscribeKeyEvents();
        }

        void OnDisable()
        {
            var keyMgr = LOLClientKeyEventManager.Instance;
            if (keyMgr != null)
            {
                keyMgr.OnSkillKey -= HandleSkillKey;
            }
        }

        private void TrySubscribeKeyEvents()
        {
            var keyMgr = LOLClientKeyEventManager.Instance;
            if (keyMgr != null)
            {
                keyMgr.OnSkillKey -= HandleSkillKey; // 防重复
                keyMgr.OnSkillKey += HandleSkillKey;
            }
        }

        private void HandleSkillKey(int slot)
        {
            switch (slot)
            {
                case 1: // Q
                    BuffManager.Add(new GailunQBuff(this));
                    break;
                case 3: // E (槽位3 对应 E)
                    weaponTrigger.Owner = this;
                    stateMachine.TransitionTo("Spell3", weaponTrigger);
                    break;
                case 4: // R
                    if (_target != null && IsTargetInAttackRange(_target))
                    {
                        stateMachine.TransitionTo("Spell4");
                    }
                    break;
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

        public GailunWeaponTrigger weaponTrigger;
    }
}
