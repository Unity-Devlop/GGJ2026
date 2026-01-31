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
    }
}