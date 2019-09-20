using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    class RockHydra : HydraHead
    {
        private const int ShockwaveMaxDamage = 30;
        private const int ShockwaveMaxKnockback = 30;
        private const int ShockwaveDamageTickRate = 20;
        private const int EpicenterRadius = 175;

        private const int RippleAmount = 3;
        private const int RippleSize = 20;
        private const int RippleSpeed = 40;
        private const int RippleDistort = 100;

        protected override float MaxAttackRange { get; } = 400;
        protected override int AttackCooldown { get; } = 240;
        protected override int AttackDuration { get; } = 90;

        protected override bool ContinueAttackWhenTargetDead { get; } = true;

        protected override void Attack()
        {
            int absTimer = Math.Abs(Timer);

            if ((absTimer - 1) % ShockwaveDamageTickRate == 0)
            {
                float minimumReach = EpicenterRadius * EpicenterRadius;
                float reachSq = minimumReach + (MaxAttackRange * MaxAttackRange - minimumReach) * (absTimer / (float)AttackDuration);

                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.active && !npc.friendly && Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        Vector2 hydraToNPC = npc.Center - projectile.Center;

                        if (hydraToNPC.X * hydraToNPC.X + hydraToNPC.Y * hydraToNPC.Y < reachSq)
                        {
                            float distance = (float)Math.Sqrt(hydraToNPC.X * hydraToNPC.X + hydraToNPC.Y * hydraToNPC.Y);
                            float intensity = 1 - (distance / MaxAttackRange);
                            npc.velocity += hydraToNPC / distance * ShockwaveMaxKnockback * intensity * npc.knockBackResist;

                            if (!npc.dontTakeDamage && !npc.immortal)
                            {
                                npc.StrikeNPC((int)(ShockwaveMaxDamage * intensity), 0, 0);
                            }
                        }
                    }
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Timer < 0)
            {
                int absTimer = Math.Abs(Timer);

                if (!Filters.Scene["HydraShockwave"].IsActive())
                {
                    Filters.Scene.Activate("HydraShockwave", projectile.position).GetShader().UseColor(RippleAmount, RippleSize, RippleSpeed).UseTargetPosition(projectile.position);
                }
                float progress = absTimer / (float)AttackDuration;
                Filters.Scene["HydraShockwave"].GetShader().UseProgress(progress).UseOpacity(RippleDistort * (1 - progress));
            }
        }

        public override void Kill(int timeLeft)
        {
            Filters.Scene["HydraShockwave"].Deactivate();
        }
    }
}
