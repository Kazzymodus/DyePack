using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Devs.Redigit
{
    class YoyoRang : ModProjectile
    {
        // Field region.

        bool isReturning = false; // Is the projectile returning to Regidit?
        int flyTime = 180; // How long the projectile will attempt to hit the target before returning.
        float projectileSpeed = 10f; // Speed of the projectile (well, what else?).

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 128;
            projectile.timeLeft = 720;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.light = 1f;
            drawOriginOffsetX = -5f;
            drawOriginOffsetY = 5;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoyorang");
        }

        public override void AI()
        {
            // ai[0] = Redigit's npc[] entry.
            // ai[1] = lifetime (for sound purposes).

            // Local variables.

            Vector2 projectileToTargetDirection; // Holds the direction from where the projectile is to where its target is.
            float projectileToTargetDistance; // Holds the distance between the projectile and the target.

            // Find Redigit, or destroys the yoyo if it fails to.

            NPC parent = Main.npc[(int)projectile.ai[0]];
            if (Main.netMode != 1 && (parent == null || parent.type != mod.NPCType("Redigit") || !parent.active))
            {
                projectile.Kill();
            }

            if (!isReturning)
            {
                // Get the direction and distance from the projectile to the target.

                projectileToTargetDirection = Main.player[parent.target].Center - projectile.Center;
                projectileToTargetDistance = (float)Math.Sqrt(projectileToTargetDirection.X * projectileToTargetDirection.X + projectileToTargetDirection.Y * projectileToTargetDirection.Y);

                // Moves the projectile towards the target.

                projectile.velocity = projectileToTargetDirection * (projectileSpeed / projectileToTargetDistance);

                flyTime--;
                if(flyTime == 0)
                {
                    isReturning = true;
                }
            }
            else
            {
                // Get the direction and distance from the projectile to Redigit.

                projectileToTargetDirection = parent.Center - projectile.Center;
                projectileToTargetDistance = (float)Math.Sqrt(projectileToTargetDirection.X * projectileToTargetDirection.X + projectileToTargetDirection.Y * projectileToTargetDirection.Y);

                // If the projectile is close enough to Redigit, destroy it. Otherwise, move it towards him.

                if (projectileToTargetDistance < 10f)
                    projectile.Kill();
                else
                    projectile.velocity = projectileToTargetDirection * (projectileSpeed / projectileToTargetDistance);
            }

            projectile.rotation += 0.3f;
            if(projectile.ai[1] % 20 == 0)
                Main.PlaySound(2, projectile.position, 1); // Plays a 'swoosh' sound.
            projectile.ai[1]++;
        }
    }
}
