using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    class LaserHydra : HydraHead
    {
        private const int ChargeTime = 180;

        protected override TargetMode TargetPriority { get; } = TargetMode.FurthestFromHydra;

        protected override int AttackCooldown { get; } = 450;
        protected override int AttackDuration { get; } = 180;

        protected override void Attack()
        {
            int absTimer = Math.Abs(Timer);

            if (absTimer < ChargeTime)
            {
                if (!HasTarget || !Main.npc[Target].active)
                {
                    AbortAttack();
                    return;
                }

                for (int i = 0; i < 2; i++)
                {
                    float xPos;
                    float yPos;

                    do
                    {
                        xPos = Main.rand.Next(-32, 33);
                        yPos = Main.rand.Next(-32, 33);
                    }
                    while (Math.Abs(xPos) + Math.Abs(yPos) < 16f);

                    Vector2 spawnPos = projectile.position + new Vector2(xPos, yPos);
                    Vector2 velocity = (projectile.position - spawnPos) * 0.075f;

                    Dust dust = Dust.NewDustPerfect(spawnPos, 156, velocity);
                    dust.noGravity = true;
                }

            }
        }
    }
}
