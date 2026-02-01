using FMODUnity;
using GGJ2026.GamePlay;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GGJ2026
{
    public class GlobalRefHolder : MonoBehaviour
    {
        public AssetReference homeScene;
        public AssetReference gameScene;
        public LevelConfig levelConfig;
        public SpriteConfig spriteConfig;
        public EventReference bgm;
        public EventReference useCard;
        public EventReference attack;
        public EventReference selectCard;
        public EventReference defence;
        public EventReference blackWhiteLaugh;
        public EventReference whiteLaugh;
        public EventReference blackLaugh;
        public EventReference dead;
        public EventReference 阎罗;
        public EventReference 阎王出场;
        public EventReference 死期已到;
        public EventReference 孟婆;
        public EventReference 受击;
        public EventReference 二郎神;
        public EventReference uiClick;
    }
}