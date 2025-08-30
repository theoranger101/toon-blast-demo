using System;
using System.Collections.Generic;
using Blocks;
using Blocks.EventImplementations;
using Blocks.UI;
using Blocks.UI.Skins;
using Data;
using DG.Tweening;
using PowerUps;
using PowerUps.EventImplementations;
using UnityEngine;
using Utilities.Events;
using Utilities.Promises;
using Random = UnityEngine.Random;

namespace Grid.UI
{
    public class GridView : MonoBehaviour
    {
        public RectTransform GridContainer;

        private Dictionary<Block, BlockView> m_ActiveBlockViews = new();

        private GlobalSettings m_Settings;

        private Sequence m_BlockMovementSequence;

        [SerializeField] private BlockSkinLibrary m_BlockSkinLibrary; // TODO: INJECT 

        private BlockViewFactory m_BlockViewFactory;

        private float m_DefaultStartPosition =>
            GridContainer.anchoredPosition.y + GridContainer.rect.height + m_Settings.BlockCellSize;

        private void Awake()
        {
            m_Settings = GlobalSettings.Get();
            m_BlockViewFactory = new BlockViewFactory(m_BlockSkinLibrary);

            // TODO: block movement to be animated and controlled by other entity
            GridRefillController.OnBlockMoved += OnBlockMoved;

            GEM.Subscribe<BlockEvent>(HandleBlockAdded, channel: (int)BlockEventType.BlockCreated);
            GEM.Subscribe<BlockEvent>(HandleBlockRemoved, channel: (int)BlockEventType.BlockPopped);
            
            GEM.Subscribe<PowerUpEvent>(OnPowerUpCreated, channel: (int)PowerUpEventType.PowerUpCreated);
        }

        private void OnDestroy()
        {
            GEM.Unsubscribe<BlockEvent>(HandleBlockAdded, channel: (int)BlockEventType.BlockCreated);
            GEM.Unsubscribe<BlockEvent>(HandleBlockRemoved, channel: (int)BlockEventType.BlockPopped);
            
            GEM.Unsubscribe<PowerUpEvent>(OnPowerUpCreated, channel: (int)PowerUpEventType.PowerUpCreated);
        }

        #region Event Handlers

        private void HandleBlockAdded(BlockEvent evt)
        {
            Debug.Log("Creating view for new block " + evt.Block.GetType() + " at position " + evt.Block.GridPosition);

            AddBlockView(evt.Block);
        }
        
        private void HandleBlockRemoved(BlockEvent evt)
        {
            var view = m_ActiveBlockViews[evt.Block];
            RemoveBlockView(view);
        }

        private void OnPowerUpCreated(PowerUpEvent evt)
        {
            evt.Tween = PlayPowerUpMerge(evt.Block, evt.BlockList, evt.PowerUpToCreate);
        }

        #endregion
        
        private void AddBlockView(Block block)
        {
            if (m_ActiveBlockViews.ContainsKey(block))
            {
                Debug.LogWarning($"View already exists for block {block}. Ignoring duplicate AddBlock.");
                return;
            }

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
        
        private void RemoveBlockView(BlockView view)
        {
            Debug.Log("Removing block view for " + view.Block.GetType() + " at position " + view.Block.GridPosition);

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

            var targetPosition = GridToAnchored(block.GridPosition);
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

        // TODO: magic numbersss
        public Tween PlayPowerUpMerge(Block pivot, IReadOnlyList<Block> mergers, PowerUpToCreate type)
        {
            var seq = DOTween.Sequence().SetRecyclable();

            if (!m_ActiveBlockViews.TryGetValue(pivot, out var pivotView))
            {
                Debug.LogWarning($"BlockView for pivot at {pivot} not found.");
                return seq;
            }

            var pivotPos = pivotView.RectTransform.anchoredPosition;
            var pivotAnimator = pivotView.GetComponent<BlockViewAnimator>(); // TODO: getcomponent at runtime!

            if (pivotAnimator == null)
            {
                Debug.LogWarning($"Animator for pivot at {pivot.GridPosition} not found.");
            }
            else
            {
                seq.Join(pivotAnimator.Bump(12f, 0.08f));
            }

            var duration = 0.08f;
            
            for (var i = 0; i < mergers.Count; i++)
            {
                var block = mergers[i];

                if (ReferenceEquals(block, pivot))
                {
                    continue;
                }

                if (!m_ActiveBlockViews.TryGetValue(block, out var blockView))
                {
                    Debug.LogWarning($"BlockView for block at {block.GridPosition} not found.");
                    continue;
                }
                
                var rt = blockView.RectTransform;
                var startPos = rt.anchoredPosition;
                var midPos = Vector2.Lerp(startPos, pivotPos, 0.5f); 
                
                var flySeq = DOTween.Sequence().SetRecyclable()
                    .Append(rt.DOAnchorPos(midPos, duration * 0.6f).SetEase(Ease.OutQuad))
                    .Append(rt.DOAnchorPos(pivotPos, duration * 0.4f).SetEase(Ease.OutCubic));

                var shrinkTween = rt.DOScale(0f, duration).SetEase(Ease.InQuad).SetRecyclable();
                var fadeTween = blockView.Image.DOFade(0f, duration).SetRecyclable();
                
                seq.Join(flySeq).Join(shrinkTween).Join(fadeTween);

                seq.AppendCallback(() =>
                {

                });
            }
            
            return seq;
        } 
    }
}