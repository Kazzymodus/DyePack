using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TCFBattles.Items.Summons
{
    class InconspicuousLookingYoyo : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 30;
            item.value = 100000;
            item.rare = 9;
            item.useAnimation = 45;
            item.useStyle = 4;
            item.useTime = 30;
            item.consumable = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inconspicuous Looking Yoyo");
            Tooltip.SetDefault("Give it a spin. Go on. What's the worst that could happen?");
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(mod.NPCType("Redigit"));
        }

        public override bool UseItem(Player player)
        {
            return true;
        }
    }
}
