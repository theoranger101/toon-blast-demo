using Blocks.BlockTypes;

namespace PowerUps.Strategies
{
    public interface IPowerUpStrategy
    {
        public PowerUpBlock Owner { get; set; }
        
        public void Activate();
    }
}