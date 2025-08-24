using Blocks;
using UnityEngine;

namespace Grid
{
    public class GridMovementController : MonoBehaviour
    {
        private void Awake()
        {
            // Subscribe to block movement events
            GridRefillController.OnBlockMoved += HandleBlockMoved;    
        }
        
        private void HandleBlockMoved(Block block)
        {
            
        }
    }
}