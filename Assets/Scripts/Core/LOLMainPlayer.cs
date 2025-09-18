using LikeLoL04.EventSystem;
using UnityEngine;

namespace LikeLoL04
{
    public class LOLMainPlayer : MonoBehaviour
    {

        public LOLGameObject mainPlayer;

        protected void Start()
        {
            mainPlayer.Camp = new Camp { Type = CampType.Blue };
        }

        protected void Update()
        {
        }
    }
}

