using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Devs.Cenx
{
    class SparkStorm : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
            //drawOffsetX = 5;
            //drawOriginOffsetY = 5;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spark Storm");
        }

        public override void AI()
        {
            // ai[0] = trigger

            if (projectile.timeLeft >= 180)
            {
                for (int i = 0; i < ((int)projectile.ai[0] / 60) + 1; i++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 64);
                }
            }
            else if (projectile.ai[0] != 0)
            {
                projectile.hostile = true;
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 27);
                projectile.ai[0] = 0;
            }
            else
            {
                Dust.NewDust(projectile.Center, projectile.width, projectile.height, 27);
            }
        }
    }
}
