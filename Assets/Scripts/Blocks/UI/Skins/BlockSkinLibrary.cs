using System.Collections.Generic;
using UnityEngine;

namespace Blocks.UI.Skins
{
    [CreateAssetMenu(menuName = "Blocks/Block Skin Library", fileName = "BlockSkinLibrary")]
    public class BlockSkinLibrary : ScriptableObject
    {
        public BlockView DefaultPrefab;
        
        [Space] 
        public List<BlockSkin> Skins = new();
        
        private readonly Dictionary<(BlockCategory, int), BlockSkin> m_Cache = new();
        
        // Key: Category, TypeId
        /// <param name="category">Category of the block as in Match, PowerUp, Obstacle, etc.</param>
        /// <param name="categoryTypeId">Related type category type cast to int. E.g.: (int)MatchBlockType for Match blocks. </param>
        /// <returns></returns>
        public BlockSkin GetSkin(BlockCategory category, int categoryTypeId)
        {
            var key = (category, typeId: categoryTypeId);
            if (m_Cache.TryGetValue(key, out var cachedSkin))
            {
                return cachedSkin;
            }

            BlockSkin foundSkin = null;
            for (var i = 0; i < Skins.Count; i++)
            {
                var skin = Skins[i];

                if (skin == null)
                {
                    continue;
                }
                if (skin.Category != category)
                {
                    continue;
                }
                
                switch (skin.Category)
                { 
                    case BlockCategory.Match:
                        if (/*skin.MatchBlockType.HasValue && */(int) skin.MatchBlockType != categoryTypeId)
                        {
                            continue;
                        }
                        break;
                    case BlockCategory.PowerUp:
                        if (/*skin.PowerUpType.HasValue && */(int)skin.PowerUpType != categoryTypeId)
                        {
                            continue;
                        }
                        break;
                    case BlockCategory.Obstacle:
                        if (/*skin.ObstacleType.HasValue && */(int)skin.ObstacleType != categoryTypeId)
                        {
                            continue;
                        }
                        break;
                }

                foundSkin = skin;
                break;
            }

            m_Cache[key] = foundSkin;
            return foundSkin;
        }
    }
}