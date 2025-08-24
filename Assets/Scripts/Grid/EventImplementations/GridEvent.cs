using System.Collections.Generic;
using Blocks;
using UnityEngine;
using Utilities.Events;

namespace Grid.EventImplementations
{
    public enum GridEventType
    {
        BlockMoved = 0,
        TriggerRefill = 1,
        RequestGrid = 2,
        RequestAxis = 3,
        AddBlock = 4,
        ClearPosition = 5,
    }
    
    public class GridEvent : Event<GridEvent>
    {
        public Block Block;
        public Vector2Int GridPosition;
        public GridAxis Axis;
        public List<Block> Blocks;

        public static GridEvent Get(Block block)
        {
            var evt = GetPooledInternal();
            evt.Block = block;
            
            return evt;
        }
        
        public static GridEvent Get(Vector2Int gridPosition)
        {
            var evt = GetPooledInternal();
            evt.GridPosition = gridPosition;
            
            return evt;
        }
        
        public static GridEvent Get(Block block, Vector2Int gridPosition)
        {
            var evt = GetPooledInternal();
            evt.Block = block;
            evt.GridPosition = gridPosition;
            
            return evt;
        }
        
        public static GridEvent Get(GridAxis axis, Vector2Int gridPosition)
        {
            var evt = GetPooledInternal();
            evt.Axis = axis;
            evt.GridPosition = gridPosition;

            return evt;
        }

        public static GridEvent Get(List<Block> blocks)
        {
            var evt = GetPooledInternal();
            evt.Blocks = blocks;

            return evt;
        }

        public override void Dispose()
        {
            base.Dispose();

            Block = null;
            GridPosition = Vector2Int.zero;
            Axis = default;
            Blocks = null;
        }
    }
}