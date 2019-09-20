using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Items.Weapons
{
    class GroundZero : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            
            item.useTime = 20;
            item.useAnimation = 5;
            item.useStyle = 5;
            item.noMelee = true;

            item.ranged = true;
            item.shoot = mod.ProjectileType("GroundZeroBeacon");
            item.shootSpeed = 6f;
            item.useAmmo = mod.ItemType("GroundZeroAmmo");
            item.autoReuse = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ground Zero");
            Tooltip.SetDefault("Fires airstrike beacons.\n\"How you say, we going to make it rain now!\"");
        }

    }
}
