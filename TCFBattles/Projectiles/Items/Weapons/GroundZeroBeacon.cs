using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Items.Weapons
{
    class GroundZeroBeacon : ModProjectile
    {
        bool isPrimed;
        Entity latchTarget;
        int bombChild;

        public override void SetDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            projectile.width = 2;
            projectile.height = 2;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.friendly = true;

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beacon");
        }

        public override void AI()
        {

            /*
            if (isPrimed)
            {
                if(latchTarget != null)
                {
                    projectile.position = latchTarget.Top;
                }

                float leadVelocity = latchTarget == null ? 0 : latchTarget.velocity.X;

                projectile.ai[0]++;

                if (projectile.ai[0] == 60)
                {
                    bombChild = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - Main.screenHeight, leadVelocity, 12, mod.ProjectileType("GroundZeroBomb"), 100, 0);
                }

                if(projectile.ai[0] >= 60 && projectile.timeLeft > 3 && (Main.projectile[bombChild] == null || !Main.projectile[bombChild].active))
                {
                    projectile.timeLeft = 3;
                }

                projectile.frameCounter++;
                if(projectile.frameCounter == 10)
                {
                    projectile.frame = projectile.frame == 0 ? 1 : 0;
                    projectile.frameCounter = 0;
                }
            }
            else
            {
                projectile.velocity.Y += 0.15f;
                if (projectile.velocity.Y > 8f)
                    projectile.velocity.Y = 8f;
                // projectile.rotation = projectile.velocity.X * 0.15f;
            }
            */

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            isPrimed = true;
            projectile.velocity = Vector2.Zero;
            if (oldVelocity.X > Math.Abs(oldVelocity.Y))
                projectile.rotation = MathHelper.ToRadians(-90);
            else if (oldVelocity.X < -Math.Abs(oldVelocity.Y))
                projectile.rotation = MathHelper.ToRadians(90);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            latchTarget = target;
            isPrimed = true;

            Vector2 targetCenter = target.Center;
            Vector2 hitDirection = target.Center - projectile.Center;

            float rotation = hitDirection.Y * 180;
            rotation *= hitDirection.X < 0 ? -1 : 1;

            projectile.rotation = rotation;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            latchTarget = target;
            isPrimed = true;
        }
    }
}
