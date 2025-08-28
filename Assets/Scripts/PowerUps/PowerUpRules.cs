using Blocks;
using Blocks.BlockTypes;
using Data;
using Grid;
using UnityEngine;

namespace PowerUps
{
    public enum PowerUpToCreate
    {
        None = 0,
        Rocket = 1,
        Bomb = 2,
        DiscoBall = 3
    }

    public readonly struct PowerUpPlan
    {
        public readonly PowerUpToCreate PowerUpToCreate;
        public readonly Vector2Int GridPos;
        
        // Optional: Rocket 
        public readonly GridAxis Orientation;
        
        // Optional: Disco Ball
        public readonly MatchBlockType TargetType;

        public PowerUpPlan(PowerUpToCreate type, Vector2Int gridPos = default, 
            GridAxis orientation = default, MatchBlockType targetType = default)
        {
            PowerUpToCreate = type;
            GridPos = gridPos;
            Orientation = orientation;
            TargetType = targetType;
        }
    }
    
    public static class PowerUpRules
    {
        public static PowerUpPlan Plan(int connectedCount, MatchBlock clickedBlock, GlobalSettings settings)
        {
            if (connectedCount >= settings.MatchCountForDiscoBall)
            {
                return new PowerUpPlan(PowerUpToCreate.DiscoBall, clickedBlock.GridPosition,
                    targetType: clickedBlock.Type);
            }
            if (connectedCount >= settings.MatchCountForBomb)
            {
                return new PowerUpPlan(PowerUpToCreate.Bomb, clickedBlock.GridPosition);
            }
            if (connectedCount >= settings.MatchCountForRocket)
            {
                return new PowerUpPlan(PowerUpToCreate.Rocket, clickedBlock.GridPosition,
                    orientation: Random.Range(0, 2) == 0 ? GridAxis.Row : GridAxis.Column);
            }
            
            return new PowerUpPlan(PowerUpToCreate.None);
        }
    }
}