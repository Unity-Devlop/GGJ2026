using System;
using GGJ2026;
using GGJ2026.GamePlay;

namespace GGJ2026.Editor
{
    [Serializable]
    public class SaveSystemEditor
    {
        public GameData data;

        public SaveSystemEditor()
        {
            if (!LocalSaveSystem.Read<GameData>(GameData.defaultDataFileName, out data))
            {
                data = new GameData();
                LocalSaveSystem.Write(GameData.defaultDataFileName, data);
            }
        }

        [Sirenix.OdinInspector.Button]
        private void Save()
        {
            LocalSaveSystem.Write(GameData.defaultDataFileName, data);
        }
    }
}