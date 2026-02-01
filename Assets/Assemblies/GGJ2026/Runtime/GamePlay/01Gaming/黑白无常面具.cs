using UnityEngine;

namespace GGJ2026.GamePlay
{
    public class 黑白无常面具 : MonoBehaviour
    {
        public GameObject blackMask;
        public GameObject whiteMask;
        public void SwitchToWhite()
        {
            blackMask.SetActive(false);
            whiteMask.SetActive(true);
        }

        public void SwitchToBlack()
        {
            blackMask.SetActive(true);
            whiteMask.SetActive(false);
        }
    }
}