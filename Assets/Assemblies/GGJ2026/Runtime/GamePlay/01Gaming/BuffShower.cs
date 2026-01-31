using System;
using System.Text;
using cfg;
using TMPro;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(IEntityController))]
    public class BuffShower : MonoBehaviour
    {
        [SerializeField] private TMP_Text buffText;
        private IEntityController entityController;

        readonly StringBuilder _stringBuilder = new StringBuilder();

        private void Awake()
        {
            entityController = GetComponent<IEntityController>();
        }


        private void Update()
        {
            entityController.GetBuffs(out var buffs);
            buffText.text = String.Empty;
            _stringBuilder.Clear();

            foreach (var buffInfo in buffs)
            {
                if (buffInfo.buffEnum == BuffEnum.死期)
                {
                    _stringBuilder.AppendLine($"死期: {(int)buffInfo.parameters}\n");
                }
                else if (buffInfo.buffEnum == BuffEnum.黑无常)
                {
                    _stringBuilder.Append("黑\n");
                }
                else if (buffInfo.buffEnum == BuffEnum.白无常)
                {
                    _stringBuilder.Append("白\n");
                }
                else
                {
                    _stringBuilder.AppendLine($"{buffInfo.buffEnum}\n");
                }
            }

            buffText.text = _stringBuilder.ToString();
        }
    }
}