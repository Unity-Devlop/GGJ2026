using cfg;
using TMPro;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    public class EnemyIntentVisual : MonoBehaviour
    {
        [SerializeField] private TMP_Text intentText;
        public void SetIntentText(IntentEnum intent)
        {
            intentText.text = intent.ToString();
        }
    }
}