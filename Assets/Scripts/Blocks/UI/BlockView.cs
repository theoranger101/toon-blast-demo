using System.Collections;
using Blocks.EventImplementations;
using Blocks.UI.Skins;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Events;

namespace Blocks.UI
{
    public class BlockView : MonoBehaviour
    {
        public RectTransform RectTransform;
        public Image Image;
        public Button Button;

        public TextMeshProUGUI GridPosText;
        
        public Block Block { get; private set; }
        private BlockSkin m_BlockSkin;
        
        public virtual void Init(Block block, BlockSkin skin)
        {
            Debug.Log("Initializing BlockView with block: " + block.GetType() + " at position " + block.GridPosition);
            
            Block = block;
            m_BlockSkin = skin;

            if (Image != null)
            {
                Image.sprite = m_BlockSkin.Icon;
            }

            if (GlobalSettings.Get().ShowGridPositions)
            {
                GridPosText.text = block.GridPosition.ToString();    
                GridPosText.enabled = true;
            }
            else
            {
                GridPosText.enabled = false;
            }
            
            SubscribeEvents();

            Button.enabled = true;
        }

        protected virtual void SubscribeEvents()
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(OnClick);
            
            GEM.Subscribe<BlockEvent>(HandleBlockPopped, (int)BlockEventType.BlockPopped);
        }
        
        protected virtual void UnsubscribeEvents()
        {
            Button.onClick.RemoveAllListeners();
            
            GEM.Unsubscribe<BlockEvent>(HandleBlockPopped, (int)BlockEventType.BlockPopped);
        }

        private void HandleBlockPopped(BlockEvent blockEvent)
        {
            if (blockEvent.Block != Block)
            {
                return;
            }
            
            OnPopped();
        }
        
        private void OnPopped()
        {
            PlayPopSequence();
            OnRelease();
        }

        protected virtual void PlayPopSequence()
        {
            if (m_BlockSkin?.PopVfxPrefab != null)
            {
                Debug.Log("Playing Pop Sequence");
            }
        }
        
        private void OnClick()
        {
            StartCoroutine(ToggleInputCoroutine());
            
            if (Block != null)
            {
                using (var clickedEvt = BlockEvent.Get(Block))
                {
                    clickedEvt.SendGlobal((int)BlockEventType.BlockClicked);
                }
            }
            else
            {
                Debug.LogWarning("Block is not assigned to BlockView.", gameObject);
            }
        }

        private void OnRelease()
        {
            Button.enabled = false;
            
            UnsubscribeEvents();
            StopAllCoroutines();
            
            Block = null;
            m_BlockSkin = null;

            RectTransform.DOKill();
            Image.DOKill();
            
            Image.DOFade(1f,0f);
            RectTransform.localScale = Vector3.one;
        }

        private void PerformClickAnimation()
        {
            // TODO: constant values and new Vector creation!
            RectTransform.DOPunchScale(new Vector2(0.9f, 0.9f), 0.15f, vibrato: 1);
        }

        // TODO: not final design
        private IEnumerator ToggleInputCoroutine()
        {
            Button.enabled = false;
            
            yield return new WaitForSeconds(0.15f);
            
            Button.enabled = true;
        }
    }
}