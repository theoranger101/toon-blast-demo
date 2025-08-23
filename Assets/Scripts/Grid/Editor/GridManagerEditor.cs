using System.Collections.Generic;
using Blocks;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Grid.Editor
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : UnityEditor.Editor
    {
        private GridManager TargetGridManager => (GridManager)target;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Initialize Grid"))
            {
                var gridData = new List<BlockSpawnData>()
                {
                    new BlockSpawnData()
                    {
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Blue,
                        GridPosition = new Vector2Int(0, 0)
                    },
                    new BlockSpawnData()
                    {
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Blue,
                        GridPosition = new Vector2Int(1, 0)
                    },
                    new BlockSpawnData()
                    {
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Blue,
                        GridPosition = new Vector2Int(2, 0)
                    },
                    new BlockSpawnData()
                    {
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Yellow,
                        GridPosition = new Vector2Int(0, 1)
                    },
                    new BlockSpawnData()
                    {
                        // Category = BlockCategory.PowerUp, PowerUpType = PowerUpType.Rocket,
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Yellow,
                        GridPosition = new Vector2Int(1, 1)
                    },
                    new BlockSpawnData()
                    {
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Yellow,
                        GridPosition = new Vector2Int(2, 1)
                    },
                    new BlockSpawnData()
                    {
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Yellow,
                        GridPosition = new Vector2Int(0, 2)
                    },
                    new BlockSpawnData()
                    {
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Green,
                        GridPosition = new Vector2Int(1, 2)
                    },
                    new BlockSpawnData()
                    {
                        Category = BlockCategory.Match, MatchBlockType = MatchBlockType.Green,
                        GridPosition = new Vector2Int(2, 2)
                    },
                };
                
                // Example usage, replace with actual grid size and spawn data
                TargetGridManager.InitGrid(new Vector2Int(3, 3), gridData);
            }

            if (GUILayout.Button("Print Grid"))
            {
                TargetGridManager.PrintGrid();
            }

            if (GUILayout.Button("Print Available Matches"))
            {
                TargetGridManager.PrintAvailableMatches();
            }

            if (GUILayout.Button("Reset Grid"))
            {
                TargetGridManager.ResetGrid();
            }
        }
    }
}

#endif