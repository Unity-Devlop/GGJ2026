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
                    // 读取旧文件内容
                    string fileContent = System.IO.File.ReadAllText(filePath);
                    var lines = new System.Collections.Generic.List<string>(fileContent.Split('\n'));

                    // 1. 移除所有现有的双斜杠注释（//）
                    // 注意：这里简单的逐行移除包含 // 的行，如果注释在代码后面可能会有问题
                    // 所以我们只移除那些以 // 开头（忽略空格）的行，以保护代码安全
                    lines.RemoveAll(l => l.TrimStart().StartsWith("//"));

                    // 2. 构造新的注释块
                    var newComments = new System.Collections.Generic.List<string>();
                    newComments.Add($"            // {desc}");
                    foreach (var (mask, card) in cardConfig.MaskToCardEffect)
                    {
                        if (mask == MaskEnum.本我) continue;
                        var targetConfig = Global.tables.CardTable.Get(card);
                        newComments.Add($"            //{mask}: {targetConfig.Desc}");
                    }

                    // 3. 寻找注入点：找到 Execute 方法的左大括号 '{' 之后的位置
                    int insertIndex = lines.FindIndex(l => l.Contains("async UniTask<bool> Execute"));
                    if (insertIndex != -1)
                    {
                        // 寻找方法名下一行的左大括号位置
                        int braceIndex = lines.FindIndex(insertIndex, l => l.Contains("{"));
                        if (braceIndex != -1)
                        {
                            // 在大括号下一行插入新注释
                            lines.InsertRange(braceIndex + 1, newComments);
                        }
                    }

                    // 写入更新后的内容
                    System.IO.File.WriteAllText(filePath, string.Join("\n", lines));
                    UnityEngine.Debug.Log($"Updated comments for: {filePath}");
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
                    "        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)");
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