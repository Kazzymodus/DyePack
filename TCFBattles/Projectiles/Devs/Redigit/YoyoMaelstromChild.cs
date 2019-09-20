using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Devs.Redigit
{
    class YoyoMaelstromChild : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.timeLeft = 180;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoyo Maelstrom");
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 1f, 0.5f, 1f);
        }
    }
}
