using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KazDyePack.Items;

namespace KazDyePack
{
    class DyePackNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.DyeTrader)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType<EmptyDyeJar>());
                nextSlot++;
            }
        }
    }
}
