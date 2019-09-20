using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TCFBattles.Items.Weapons
{
    class GroundZeroAmmo : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 50;
            item.ammo = item.type;
            item.consumable = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Airstrike Beacon");
        }
    }
}
