using Blocks.EventImplementations;
using Blocks.UI.Skins;
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
        
        public Block Block { get; private set; }
        private BlockSkin m_BlockSkin;
        
        public virtual void Init(Block block, BlockSkin skin)
        {
            Debug.Log("Initializing BlockView with block: " + block.GetType());
            
            Block = block;
            m_BlockSkin = skin;

            if (Image != null)
            {
                Image.sprite = m_BlockSkin.Icon;
                Image.SetNativeSize();
            }
            
            SubscribeEvents();
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
        
        public void OnPopped()
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
            if (Block != null)
            {
                using (var clickedEvt = BlockEvent.Get(Block))
                {
                    clickedEvt.SendGlobal((int)BlockEventType.BlockClicked);
                }
            }
            else
            {
                Debug.LogWarning("Block is not assigned to BlockView.");
            }
        }

        private void OnRelease()
        {
            /*
            using (var releaseEvt = BlockEvent.Get(this))
            {
                releaseEvt.SendGlobal((int)BlockEventType.BlockDestroyed);
            }
            */
            
            UnsubscribeEvents();
            
            Block = null;
            m_BlockSkin = null;
        }
    }
}