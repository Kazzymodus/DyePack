using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Items.Weapons
{
    class GroundZeroBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.tileCollide = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ground Zero");
        }

        public override void AI()
        {

        }
    }
}
