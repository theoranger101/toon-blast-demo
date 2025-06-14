using UnityEngine;

namespace Data
{
    // TODO: consider a different approach for global settings

    [CreateAssetMenu]
    public class GlobalSettings : ScriptableObject
    {
        private static GlobalSettings s_Settings;

        private static GlobalSettings Settings
        {
            get
            {
                if (!s_Settings)
                {
                    s_Settings = Resources.Load<GlobalSettings>("Settings/GlobalSettings");
                }

                return s_Settings;
            }
        }

        public static GlobalSettings Get()
        {
            return Settings;
        }

        public int GridWidth = 8;
        public int GridHeight = 9;
        
        public int MatchCountForRocket = 5;
        public int MatchCountForBomb = 7;
        public int MatchCountForDiscoBall = 10;
        
        public GameObject MatchBlockPrefab;
        
        public float BlockCellSize = 100f;
        public float BlockCellSpacing = 10f;
    }
}