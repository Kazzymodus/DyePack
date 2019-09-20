using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Devs.Cenx
{
    class StarStrikeChild : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.timeLeft = 120;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Strike");
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 1f, 1f, 1f);
            projectile.rotation += 0.2f;
            projectile.alpha = 255 - (int)(255f * (projectile.timeLeft / 120f));
        }
    }
}
