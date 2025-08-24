using System;
using PowerUps.Strategies;

namespace Blocks
{
    public class PowerUpBlock : Block
    {
        public PowerUpType Type;
        
        public override bool IsAffectedByGravity => true;
        public override bool CanBePopped => true;

        private IPowerUpStrategy m_Strategy;

        public override void Init(BlockSpawnData spawnData)
        {
            Type = spawnData.PowerUpType ?? throw new Exception("PowerUpType is required for PowerUpBlock");
            GridPosition = spawnData.GridPosition;

            m_Strategy = PowerUpStrategyFactory.GetStrategy(Type);
            m_Strategy.Owner = this;
        }

        public override void OnAffectedByPowerUp()
        {
            throw new System.NotImplementedException();
        }

        public override void Pop()
        {
            base.Pop();
            m_Strategy.Activate();
        }
    }
}