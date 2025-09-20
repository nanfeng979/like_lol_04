namespace LikeLoL04
{
    public class Buff
    {
        protected float timer { get; set; }

        protected LOLGameObject owner { get; set; }

        protected bool islimited { get; set; }

        protected float duration { get; set; }

        public Buff(LOLGameObject owner)
        {
            this.owner = owner;
            timer = 0f;
        }

        public virtual void OnApply()
        {
        }

        public virtual void OnUpdate(float deltaTime)
        {
            timer += deltaTime;
        }

        public virtual void OnRemove()
        {
        }
        
        public bool IsExpired()
        {
            if (islimited && timer >= duration)
            {
                return true;
            }
            return false;
        }
    }
}