using Blocks;
using Blocks.BlockTypes;

namespace PowerUps.Strategies
{
    public interface IPowerUpStrategy
    {
        public PowerUpType PowerUpType { get; }
        
        public PowerUpBlock Owner { get; set; }

        public void Plan(PowerUpResolver resolver);

        public void Reset();
    }
}