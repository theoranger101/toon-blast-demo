using System.Collections.Generic;
using Blocks;
using LevelManagement.Data;
using UnityEngine;
using Utilities;
using Utilities.Pooling;

namespace LevelManagement
{
    public static class LevelLoader
    {
        public static List<BlockSpawnData> BuildSpawnData(LevelDefinition lvlDef, int? runSeed = null)
        {
            var seed = runSeed ?? lvlDef.Seed;
            var rng = new System.Random(seed);
            
            var groupIds = HashSetPool<int>.Get();
            for (var i = 0; i < lvlDef.Cells.Count; i++)
            {
                var cell = lvlDef.Cells[i];
                if (cell.CellType != CellType.MatchBlock || cell.MatchGroupId < 0)
                {
                    continue;
                }

                groupIds.Add(cell.MatchGroupId);
            }

            var colors = ListPool<MatchBlockType>.Get(lvlDef.Palette);
            colors.Shuffle(rng);

            var groupToColor = DictionaryPool<int, MatchBlockType>.Get();
            var colorIndex = 0;
            foreach (var groupId in groupIds)
            {
                groupToColor.Add(groupId, colors[colorIndex % colors.Count]);
                colorIndex++;
            }
            
            var spawnData = ListPool<BlockSpawnData>.Get();
            for (var i = 0; i < lvlDef.Cells.Count; i++)
            {
                var cell = lvlDef.Cells[i];

                var pos = cell.Position;
                var data = new BlockSpawnData { GridPosition = pos };

                switch (cell.CellType)
                {
                    case CellType.MatchBlock:
                        data.Category = BlockCategory.Match;
                        data.MatchBlockType = groupToColor[cell.MatchGroupId];
                        
                        break;
                    case CellType.ObstacleBlock:
                        data.Category = BlockCategory.Obstacle;
                        data.ObstacleType = cell.ObstacleType;
                        
                        break;
                    case CellType.PowerUpBlock:
                        data.Category = BlockCategory.PowerUp;
                        data.PowerUpType = cell.PowerUpType;
                        
                        break;
                    case CellType.Empty:
                        continue;
                }
                
                spawnData.Add(data);
            }
            
            HashSetPool<int>.Release(groupIds);
            ListPool<MatchBlockType>.Release(colors);
            
            return spawnData;
        }
    }
}