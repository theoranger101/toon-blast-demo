using System;
using Blocks;

namespace PowerUps.Strategies
{
    public class PowerUpStrategyFactory
    {
        public static IPowerUpStrategy GetStrategy(PowerUpType type) => type switch
        {
            PowerUpType.Rocket => new RocketPowerUpStrategy(),
            PowerUpType.Bomb => new BombPowerUpStrategy(),
            PowerUpType.DiscoBall => new DiscoBallPowerUpStrategy(),
            _ => throw new Exception($"No strategy has been implemented for power up type {type}")
        };
    }
}