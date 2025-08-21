using System;
using UnityEngine;

namespace Blocks
{
    public class MatchBlock : Block
    {
        public MatchBlockType Type;
        
        public override bool IsAffectedByGravity => true;
        public override bool CanBePopped => true;

        public override void Init(BlockSpawnData spawnData)
        {
            Type = spawnData.MatchBlockType ?? throw new Exception("MatchBlockType is required for MatchBlock");
            GridPosition = spawnData.GridPosition;
        }
        
        public override void OnAffectedByPowerUp()
        {
            throw new System.NotImplementedException();
        }

        public override void Pop()
        {
            base.Pop();
            Debug.Log("Popped MatchBlock: " + Type + " at position " + GridPosition + ".");
        }
    }
}