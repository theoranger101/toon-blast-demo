using Blocks.EventImplementations;
using UnityEngine;
using Utilities.Events;

namespace Blocks
{
    public enum BlockCategory
    {
        Match = 0,
        PowerUp = 1,
        Obstacle = 2,
    }
    
    public enum MatchBlockType
    {
        Blue = 0,
        Green = 1,
        Red = 2,
        Yellow = 3,
    }

    public enum PowerUpType
    {
        Rocket = 0,
        Bomb = 1,
        DiscoBall = 2,
    }

    public enum ObstacleType
    {
        Balloon = 0,
        WoodenBox = 1,
    }

    public abstract class Block
    {
        public Vector2Int GridPosition;
        
        public abstract bool IsAffectedByGravity { get; }
        public abstract bool CanBePopped { get; }
        public bool IsPopped { get; protected set; }
        public abstract void Init(BlockSpawnData spawnData);
        public abstract void OnAffectedByPowerUp();
        
        public virtual void Pop()
        {
            if (IsPopped)
            {
                Debug.LogWarning("Trying to pop a block that has already been popped. Block at position " +
                                 GridPosition + " is already popped.");
            }
            
            Debug.Log("Popped Block at position " + GridPosition + ".");
            using (var poppedEvt = BlockEvent.Get(this))
            {
                poppedEvt.SendGlobal((int)BlockEventType.BlockPopped);
            }
            
            IsPopped = true;
        }
    }
}
