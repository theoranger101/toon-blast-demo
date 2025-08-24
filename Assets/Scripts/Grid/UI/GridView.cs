using System.Collections.Generic;
using Blocks;
using Blocks.EventImplementations;
using Blocks.UI;
using Blocks.UI.Skins;
using Data;
using DG.Tweening;
using UnityEngine;
using Utilities.Events;

namespace Grid.UI
{
    public class GridView : MonoBehaviour
    {
        public RectTransform GridContainer;
        
        private Dictionary<Block, BlockView> m_ActiveBlockViews = new();
        
        private GlobalSettings m_Settings;
        
        private Sequence m_BlockMovementSequence;
        
        [SerializeField] 
        private BlockSkinLibrary  m_BlockSkinLibrary; // TODO: INJECT 
        
        private BlockViewFactory m_BlockViewFactory; 

        private float m_DefaultStartPosition =>
            GridContainer.anchoredPosition.y + GridContainer.rect.height + m_Settings.BlockCellSize;
        
        private void Awake()
        {
            m_Settings = GlobalSettings.Get();
            m_BlockViewFactory = new BlockViewFactory(m_BlockSkinLibrary);
            
            // TODO: block movement to be animated and controlled by other entity
            GridRefillController.OnBlockMoved += OnBlockMoved;
            
            GEM.Subscribe<BlockEvent>(OnBlockAdded, channel:(int)BlockEventType.BlockCreated);
            GEM.Subscribe<BlockEvent>(OnBlockRemoved, channel:(int)BlockEventType.BlockPopped);
        }

        private void OnBlockAdded(BlockEvent evt)
        {
            Debug.Log("Creating view for new block");
            
            OnBlockAdded(evt.Block);
        }
        
        private void OnBlockAdded(Block block)
        {
            var view = m_BlockViewFactory.SpawnView(block, GridContainer);

            if (view == null)
            {
                return;
            }
            
            var targetPos = GridToAnchored(block.GridPosition);

            var startPos = m_DefaultStartPosition * Vector2.up + targetPos; // TODO: can definitely be improved
            view.RectTransform.anchoredPosition = startPos;
            
            MoveBlockView(view, targetPos);
            
            m_ActiveBlockViews.Add(block, view);
        }

        private void OnBlockRemoved(BlockEvent evt)
        {
            var view = m_ActiveBlockViews[evt.Block];
            RemoveBlockView(view);
        }
        
        private void RemoveBlockView(BlockView view)
        {
            Debug.Log("Removing block view");
            
            m_BlockViewFactory.ReleaseView(view);
            m_ActiveBlockViews.Remove(view.Block);
        }

        private Vector2 GridToAnchored(Vector2Int gridPosition)
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
            
            var targetPosition =  GridToAnchored(block.GridPosition);
            MoveBlockView(blockView, targetPosition); 
        }
        
        private void MoveBlockView(BlockView view, Vector2 targetPos)
        {
            if (m_BlockMovementSequence == null || !m_BlockMovementSequence.IsActive())
            {
                m_BlockMovementSequence = DOTween.Sequence();
            }
            
            m_BlockMovementSequence.Join(view.RectTransform.DOAnchorPos(targetPos,
                m_Settings.BlockMovementDuration, true).SetEase(Ease.OutQuad).SetRecyclable());
        }
    }
}