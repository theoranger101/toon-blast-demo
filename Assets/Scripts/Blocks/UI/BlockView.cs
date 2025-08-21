using System;
using UnityEngine;
using UnityEngine.UI;

namespace Blocks.UI
{
    public class BlockView : MonoBehaviour
    {
        public RectTransform RectTransform;
        public Image Image;
        public Button Button;
        
        public Block Block;
        
        public static event Action<BlockView> OnBlockViewCreated;
        public static event Action<BlockView> OnBlockViewDestroyed;
        
        public static event Action<Block> OnBlockClicked;

        public void Init(Block block)
        {
            Debug.Log("Initializing BlockView with block: " + block.GetType().Name);
            
            Block = block;
            
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

            block.OnPopped += OnPopped;
            Button.onClick.AddListener(OnClick);
            
            OnBlockViewCreated?.Invoke(this);
        }

        public void OnPopped(Block block)
        {
            OnBlockViewDestroyed?.Invoke(this);
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

        private void OnDestroy()
        {
            Button.onClick.RemoveAllListeners();
        }
    }
}