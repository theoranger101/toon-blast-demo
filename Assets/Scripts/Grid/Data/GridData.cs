using System.Collections.Generic;
using Blocks;
using UnityEngine;

namespace Grid.Data
{
    [CreateAssetMenu(fileName = "New Grid Data", menuName = "Grid/Grid Data")]
    public class GridData : ScriptableObject
    {
        public List<BlockSpawnData> spawnData = new();
    }
}