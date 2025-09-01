using System.Collections.Generic;
using LevelManagement.Data;
using UnityEngine;

namespace LevelManagement
{
    // TODO: Temporary implementation
    public class LevelManager : MonoBehaviour
    {
        public List<LevelDefinition> LevelDefinitions;
        public LevelController LevelController;
        
        public int CurrentLevel;

        private void Start()
        {
            StartLevel(0);
        }

        public void StartLevel(int? level = null)
        {
            var lvl = level ?? CurrentLevel;

            if (lvl >= LevelDefinitions.Count)
            {
                Debug.LogError($"Level {lvl} is out of range.");
                return;
            }
            
            LevelController.StartLevel(LevelDefinitions[lvl]);
        }
    }
}