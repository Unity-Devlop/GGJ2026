using System.Text;

namespace GGJ2026.Editor
{
    public class CardEffectGeneratorEditor
    {
        [Sirenix.OdinInspector.Button]
        private void Generate()
        {
            string path = "Assets/Assemblies/GGJ2026/Runtime/GamePlay/01Gaming/CardEffects";

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var cardConfig in Global.tables.CardTable.DataList)
            {
                string name = cardConfig.Id.ToString();
                string desc = cardConfig.Desc;
                string filePath = $"{path}/{(int)(cardConfig.Id):0000}{name}.cs";
                if (System.IO.File.Exists(filePath))
                {
                    UnityEngine.Debug.LogWarning($"File already exists: {filePath}");
                    continue;
                }

                stringBuilder.Clear();
                stringBuilder.AppendLine("using cfg;");
                stringBuilder.AppendLine("using Cysharp.Threading.Tasks;");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("namespace GGJ2026.GamePlay");
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine($"    [CardExecutor(CardEnum.{name})]");
                stringBuilder.AppendLine($"    public class {name} : ICardEffectExecutor");
                stringBuilder.AppendLine("    {");
                stringBuilder.AppendLine(
                    "        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)");
                stringBuilder.AppendLine("        {");
                stringBuilder.AppendLine($"            // {desc}");
                stringBuilder.AppendLine("            return false;");
                stringBuilder.AppendLine("        }");
                stringBuilder.AppendLine("    }");
                stringBuilder.AppendLine("}");

                System.IO.File.WriteAllText(filePath, stringBuilder.ToString());
                UnityEngine.Debug.Log($"Generated file: {filePath}");
            }
        }
    }
}