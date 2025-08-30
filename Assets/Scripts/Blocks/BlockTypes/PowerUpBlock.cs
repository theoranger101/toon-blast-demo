using System;
using PowerUps.Strategies;
using UnityEngine;

namespace Blocks.BlockTypes
{
    public class PowerUpBlock : Block
    {
        public PowerUpType Type;
        
        public override bool IsAffectedByGravity { get; protected set; } = true;
        public override bool CanBePopped => true;

        public IPowerUpStrategy Strategy { get; private set; }
        
        public override void Init(in BlockSpawnData spawnData)
        {
            Type = spawnData.PowerUpType ?? throw new Exception("PowerUpType is required for PowerUpBlock");
            GridPosition = spawnData.GridPosition;
            
            Strategy = PowerUpStrategyFactory.GetStrategy(Type);
            Strategy.Owner = this;
        }

        public override void Pop()
        {
            if (IsPopped)
            {
                Debug.LogWarning("Trying to pop a block that has already been popped. Block at position " +
                                 GridPosition + " is already popped.");
                return;
            }

            Debug.Log($"Activated PowerUpBlock {Type} at position {GridPosition}");
            
            Strategy.Activate();
            base.Pop();
        }

        public override void Release()
        {
            base.Release();
            // TODO: add strategy pooling -> Strategy.Release();
            Strategy = null;
            Type = default;
        }
    }
}