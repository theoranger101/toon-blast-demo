using System.Collections.Generic;
using Blocks;
using Blocks.UI;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Grid.UI
{
    public class GridView : MonoBehaviour
    {
        public RectTransform GridContainer;
        
        private Dictionary<Block, BlockView> m_ActiveBlockViews = new();
        
        private GlobalSettings m_Settings;
        
        private Sequence m_BlockMovementSequence;

        private float m_DefaultStartPosition =>
            GridContainer.anchoredPosition.y + GridContainer.rect.height + m_Settings.BlockCellSize;
        
        private void Awake()
        {
            m_Settings = GlobalSettings.Get();

            BlockView.OnBlockViewCreated += AddBlockView;
            BlockView.OnBlockViewDestroyed += RemoveBlockView;
            
            // TODO: block movement to be animated and controlled by other entity
            GridRefillController.OnBlockMoved += OnBlockMoved;
        }

        private void AddBlockView(BlockView view)
        {
            view.RectTransform.SetParent(GridContainer);

            var viewGridPos = view.Block.GridPosition;
            var anchoredPosition = GetGridToAnchoredPosition(viewGridPos);
            view.RectTransform.anchoredPosition = m_DefaultStartPosition * Vector2.up + anchoredPosition;

            MoveBlockView(view, anchoredPosition);
            
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
            if (!m_ActiveBlockViews.TryGetValue(block, out var blockView))
            {
                Debug.LogWarning($"BlockView for block at {block.GridPosition} not found.");
                return;
            }
            
            var targetPosition =  GetGridToAnchoredPosition(block.GridPosition);
            MoveBlockView(blockView, targetPosition); 
        }

        private void MoveBlockView(BlockView view, Vector2 targetPos)
        {
            if (m_BlockMovementSequence == null || !m_BlockMovementSequence.IsActive())
            {
                m_BlockMovementSequence = DOTween.Sequence();
            }
            
            m_BlockMovementSequence.Join(view.RectTransform.DOAnchorPos(targetPos,
                m_Settings.BlockMovementDuration, true).SetEase(Ease.OutBounce).SetRecyclable());
        }
    }
}