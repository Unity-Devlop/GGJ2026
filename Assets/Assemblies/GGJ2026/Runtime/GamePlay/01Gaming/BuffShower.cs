using System;
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

        private void Awake()
        {
            entityController = GetComponent<IEntityController>();
        }

        private void Update()
        {
            entityController.GetBuffs(out var buffs);
            buffText.text = String.Empty;
            foreach (var buffInfo in buffs)
            {
                if (buffInfo.buffEnum == BuffEnum.死期)
                {
                    buffText.text = $"死期: {(int)buffInfo.parameters}";
                    return;
                }
            }
        }
    }
}