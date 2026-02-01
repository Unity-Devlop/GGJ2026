using System;
using System.Collections.Generic;
using cfg;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [CreateAssetMenu(fileName = "SpriteConfig", menuName = "GGJ2026/Config/SpriteConfig")]
    public class SpriteConfig : ScriptableObject
    {
        public SerializableDictionary<MaskEnum, Sprite> broaderMaskSprites;

        private void OnValidate()
        {
            foreach (var maskEnum in EnumHelper<MaskEnum>.keys)
            {
                if (!broaderMaskSprites.TryAdd(maskEnum, null)) continue;
            }
        }

        public SerializableDictionary<CardEnum, Sprite> cardMangaSprites;
        public SerializableDictionary<CardTypeEnum, Sprite> cardTypeSprites;
        public SerializableDictionary<MaskEnum, Sprite> maskSprites;
        public Sprite[] mask黑白无常Icon = new Sprite[2];
    }
}