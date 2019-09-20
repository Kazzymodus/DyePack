using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Devs.Cenx
{
    class StarStrike : ModProjectile
    {
        int childProjectileSpeed = 10;
        int childProjectileAngle = 72;

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.timeLeft = 180;
            projectile.tileCollide = false;
            projectile.hostile = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Strike");
        }

        public override void AI()
        {
            // ai[0] = timer
            // ai[1] = target

            Lighting.AddLight(projectile.position, 1f, 1f, 1f);
            projectile.rotation += 0.2f;

            if (projectile.ai[0] > 20)
            {
                projectile.tileCollide = true;
            }

            if(projectile.position.Y > Main.player[(int)projectile.ai[1]].Center.Y + 400)
            {
                projectile.Kill();
            }

            projectile.ai[0]++;
            
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, projectile.position, 30);
            for (int i = 0; i < 5; i++)
            {
                float speedX = (float)Math.Sin(projectile.rotation + childProjectileAngle * i * (Math.PI / 180f)) * childProjectileSpeed;
                float speedY = (float)Math.Cos(projectile.rotation + childProjectileAngle * i * (Math.PI / 180f)) * childProjectileSpeed;
                Projectile.NewProjectile(projectile.position.X, projectile.position.Y, speedX, speedY, mod.ProjectileType("StarStrikeChild"), 1, 0);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }
    }
}
