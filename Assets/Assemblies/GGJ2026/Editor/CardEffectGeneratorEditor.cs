using System.Text;
using cfg;

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
                    // 把里面的注释换一下 和下面一样的逻辑
                    string fileContent = System.IO.File.ReadAllText(filePath);
                    
                    // Remove All "//" comments 然后重新注释上
                    var lines = fileContent.Split('\n');



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

                foreach (var (mask, card) in cardConfig.MaskToCardEffect)
                {
                    if (mask == MaskEnum.本我) continue;
                    var targetConfig = Global.tables.CardTable.Get(card);
                    stringBuilder.AppendLine($"            //{mask}: {targetConfig.Desc}");
                }

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