namespace LikeLoL04
{
    public class LOLRedPlayer : LOLGameObject
    {

        protected override void Start()
        {
            base.Start();
            Camp = new Camp { Type = CampType.Red };
        }

        protected override void Update()
        {
            base.Update();
        }

    }
}