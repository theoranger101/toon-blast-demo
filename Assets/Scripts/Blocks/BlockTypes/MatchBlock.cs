using System;
using UnityEngine;

namespace Blocks.BlockTypes
{
    public class MatchBlock : Block
    {
        public MatchBlockType Type;
        
        public override bool IsAffectedByGravity { get; protected set; } = true;
        public override bool CanBePopped => true;

        public override void Init(in BlockSpawnData spawnData)
        {
            Type = spawnData.MatchBlockType ?? throw new Exception("MatchBlockType is required for MatchBlock");
            GridPosition = spawnData.GridPosition;
        }

        public override void Pop()
        {
            Debug.Log($"Popped MatchBlock: {Type} at position {GridPosition}.");

            base.Pop();
        }

        public override void Release()
        {
            base.Release();
            
            Type = default;
        }
    }
}