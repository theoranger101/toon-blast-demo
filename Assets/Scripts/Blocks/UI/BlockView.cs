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
        public static event Action<Block> OnBlockMoved; 

        public void Init(Block block)
        {
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
            else
            {
                // Debug.LogWarning("Block type is not MatchBlock, no sprite assigned.");
            }

            block.OnBlockPopped += OnPopped;
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