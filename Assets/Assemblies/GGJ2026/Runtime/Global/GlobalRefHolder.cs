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
    }
}