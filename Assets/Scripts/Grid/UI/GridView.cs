using System.Collections.Generic;
using Blocks;
using Blocks.UI;
using Data;
using UnityEngine;

namespace Grid.UI
{
    public class GridView : MonoBehaviour
    {
        public RectTransform GridContainer;
        
        private Dictionary<Block, BlockView> m_ActiveBlockViews = new();
        
        private GlobalSettings m_Settings;

        private void Awake()
        {
            m_Settings = GlobalSettings.Get();

            BlockView.OnBlockViewCreated += AddBlockView;
            BlockView.OnBlockViewDestroyed += RemoveBlockView;
            
            GridManager.OnBlockMoved += OnBlockMoved;
        }

        private void AddBlockView(BlockView view)
        {
            view.RectTransform.SetParent(GridContainer);

            var viewGridPos = view.Block.GridPosition;
            var anchoredPosition = GetGridToAnchoredPosition(viewGridPos);
            view.RectTransform.anchoredPosition = anchoredPosition;
            
            m_ActiveBlockViews.Add(view.Block, view);
        }

        private void RemoveBlockView(BlockView view)
        {
            m_ActiveBlockViews.Remove(view.Block);
            
            // TODO: Consider using a pooling system instead of destroying the view
            Destroy(view.gameObject);
        }

        private Vector2 GetGridToAnchoredPosition(Vector2Int gridPosition)
        {
            return new Vector2(gridPosition.x * (m_Settings.BlockCellSize + m_Settings.BlockCellSpacing),
                gridPosition.y * (m_Settings.BlockCellSize + m_Settings.BlockCellSpacing));
        }

        private void OnBlockMoved(Block block)
        {
            // TODO: gravity animation to be added
            m_ActiveBlockViews[block].RectTransform.anchoredPosition = GetGridToAnchoredPosition(block.GridPosition);
        }
    }
}