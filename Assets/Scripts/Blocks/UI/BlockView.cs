using System;
using Blocks.UI.Skins;
using UnityEngine;
using UnityEngine.UI;

namespace Blocks.UI
{
    public class BlockView : MonoBehaviour
    {
        public RectTransform RectTransform;
        public Image Image;
        public Button Button;
        
        public Block Block { get; private set; }
        private BlockSkin m_BlockSkin;
        
        public static event Action<BlockView> OnBlockViewCreated;
        public static event Action<BlockView> OnBlockViewDestroyed;
        
        public static event Action<Block> OnBlockClicked;
        
        private event Action<BlockView> ReleaseBlockView;

        public virtual void Init(Block block, BlockSkin skin, Action<BlockView> releaseToPool)
        {
            Debug.Log("Initializing BlockView with block: " + block.GetType());
            
            Block = block;
            m_BlockSkin = skin;

            if (Image != null)
            {
                Image.sprite = m_BlockSkin.Icon;
                Image.SetNativeSize();
            }
            
            /*
            // TODO: dummy implementation for testing purposes
            if(Block is MatchBlock matchBlock)
            {
                Image.color = matchBlock.Type switch
                {
                    MatchBlockType.Blue => Color.blue,
                    MatchBlockType.Green => Color.green,
                    MatchBlockType.Red => Color.red,
                    MatchBlockType.Yellow => Color.yellow,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else if(block is PowerUpBlock powerUpBlock)
            {
                Image.color = powerUpBlock.Type switch
                {
                    PowerUpType.Rocket => Color.cyan,
                    PowerUpType.Bomb => Color.magenta,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            */

            Button.onClick.RemoveAllListeners();
            
            block.OnPopped += OnPopped;
            Button.onClick.AddListener(OnClick);
            
            OnBlockViewCreated?.Invoke(this);
            
            ReleaseBlockView = releaseToPool;
            
            gameObject.SetActive(true);
        }

        public void OnPopped(Block block)
        {
            OnBlockViewDestroyed?.Invoke(this);
            
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
                OnBlockClicked?.Invoke(Block);
            }
            else
            {
                Debug.LogWarning("Block is not assigned to BlockView.");
            }
        }

        private void OnRelease()
        {
            Button.onClick.RemoveAllListeners();
            Block.OnPopped -= OnPopped;
            
            Block = null;
            m_BlockSkin = null;
            
            ReleaseBlockView?.Invoke(this);
        }
    }
}