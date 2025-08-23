using UnityEngine;

namespace Blocks.UI.Skins
{
    [CreateAssetMenu(menuName = "Blocks/Block Skin", fileName = "BlockSkin")]
    public class BlockSkin : ScriptableObject
    {
        [Header("Match Criteria")] 
        public BlockCategory Category;
        public MatchBlockType MatchBlockType;
        public PowerUpType PowerUpType;
        public ObstacleType ObstacleType;
        
        [Header("Prefab & Icon")]
        [Tooltip("Optional: a dedicated prefab that inherits from BlockView. If left null, factory will opt to default prefab.")]
        public BlockView OverridePrefab;
        public Sprite Icon;
        public Vector2 IconPivot = new Vector2(0.5f, 0.5f);
        
        [Header("VFX")]
        public GameObject PopVfxPrefab;
    }
}