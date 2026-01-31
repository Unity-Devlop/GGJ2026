using cfg;

namespace GGJ2026.GamePlay
{
    public struct PlayerSwitchMaskOperation : IOperation
    {
        public MaskEnum id;

        public PlayerSwitchMaskOperation(MaskEnum id)
        {
            this.id = id;
        }
    }
}