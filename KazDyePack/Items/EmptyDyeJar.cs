using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace KazDyePack.Items
{
    class EmptyDyeJar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Can be used to make dyes from the Dye Pack");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.value = 100;
        }
    }
}
