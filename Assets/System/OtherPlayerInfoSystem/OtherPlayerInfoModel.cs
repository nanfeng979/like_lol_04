using UnityEngine;

namespace LikeLoL04
{
    public class OtherPlayerInfoModel
    {
        public LOLGameObject Source { get; private set; }
        public Sprite Avatar { get; private set; }
        public int AttackValue { get; private set; }

        public void FromLOLGameObject(LOLGameObject obj)
        {
            Source = obj;
            if (obj != null && obj.Data != null)
            {
                Avatar = obj.Data.Avatar;
                AttackValue = obj.Data.AttackValue;
            }
            else
            {
                Avatar = null;
                AttackValue = 0;
            }
        }
    }
}