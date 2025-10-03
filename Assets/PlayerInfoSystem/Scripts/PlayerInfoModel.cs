using UnityEngine;

namespace LikeLoL04
{
    public class PlayerInfoModel
    {
        public LOLGameObject Source { get; private set; }
        public float CurrentHp { get; set; }
        public float MaxHp { get; set; }

        public Sprite Avatar { get; private set; }

        public LOLGameObjectAttributes Attributes { get; private set; }

        public void FromLOLGameObject(LOLGameObject obj)
        {
            Source = obj;
            if (obj != null && obj.Data != null)
            {
                CurrentHp = obj.Data.CurrentHp;
                MaxHp = obj.Data.MaxHp;
                Avatar = obj.Data.Avatar;
                Attributes = obj.Data.Attributes;
            }
            else
            {
                CurrentHp = 0;
                MaxHp = 0;
                Avatar = null;
                Attributes = null;
            }
        }

        public void UpdateHp(int currentHp, int maxHp)
        {
            CurrentHp = currentHp;
            MaxHp = maxHp;
        }
    }
}