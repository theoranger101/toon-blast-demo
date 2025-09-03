using System;
using System.Collections.Generic;
using Blocks;
using Blocks.BlockTypes;
using UnityEngine;

namespace PowerUps.Strategies
{
    public static class PowerUpStrategyFactory
    {
        private static readonly int s_InitialCapacity = 5;
        private static readonly Dictionary<PowerUpType, Queue<IPowerUpStrategy>> s_StrategyPool = new()
        {
            { PowerUpType.Rocket, new Queue<IPowerUpStrategy>(s_InitialCapacity) },
            { PowerUpType.Bomb, new Queue<IPowerUpStrategy>(s_InitialCapacity) },
            { PowerUpType.DiscoBall, new Queue<IPowerUpStrategy>(s_InitialCapacity) }
        };

        public static IPowerUpStrategy GetStrategy(PowerUpType type)
        {
            if (!s_StrategyPool.TryGetValue(type, out var pool))
            {
                Debug.LogError($"Invalid Power Up Type {type}");
                return null;
            }

            if (pool.Count > 0)
            {
                var strategy = pool.Dequeue();
                strategy.Reset();
                
                return strategy;
            }
            
            return CreateStrategy(type);
        }
        
        public static IPowerUpStrategy GetStrategy(PowerUpType type, PowerUpBlock owner)
        {
            var strategy = GetStrategy(type);
            strategy.Owner = owner;
            
            return strategy;
        }

        public static void ReleaseStrategy(IPowerUpStrategy strategy)
        {
            if (strategy == null)
            {
                return;
            }
            
            strategy.Reset();
            s_StrategyPool[strategy.PowerUpType].Enqueue(strategy);
        }
        
        private static IPowerUpStrategy CreateStrategy(PowerUpType type) => type switch
        {
            PowerUpType.Rocket => new RocketPowerUpStrategy(),
            PowerUpType.Bomb => new BombPowerUpStrategy(),
            PowerUpType.DiscoBall => new DiscoBallPowerUpStrategy(),
            _ => throw new Exception($"No strategy has been implemented for power up type {type}")
        };

    }
}