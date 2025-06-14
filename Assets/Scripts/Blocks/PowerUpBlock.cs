using System;

namespace Blocks
{
    public class PowerUpBlock : Block
    {
        public PowerUpType Type;
        
        public override bool IsAffectedByGravity => true;
        public override bool CanBePopped => true;

        public override void Init(BlockSpawnData spawnData)
        {
            Type = spawnData.PowerUpType ?? throw new Exception("PowerUpType is required for PowerUpBlock");
            GridPosition = spawnData.GridPosition;
        }

        public override void OnAffectedByPowerUp()
        {
            throw new System.NotImplementedException();
        }

        public override void Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}