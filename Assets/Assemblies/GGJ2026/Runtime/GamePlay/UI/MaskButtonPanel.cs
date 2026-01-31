using System;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    public class MaskButtonPanel : MonoBehaviour
    {
        [SerializeField] private GameObject maskObject;
        private void Awake()
        {
            foreach (var maskConfig in Global.tables.MaskTable.DataList)
            {
                if (maskConfig.CanWearAnyTime)
                {
                    var maskObj = GameObject.Instantiate(maskObject, this.transform);
                    var button = maskObj.GetComponent<SwitchMaskButton>();
                    button.Bind(maskConfig.Id);
                }
            }
        }
    }
}