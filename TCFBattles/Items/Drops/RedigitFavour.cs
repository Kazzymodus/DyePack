using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace TCFBattles.Items.Drops
{
    class RedigitFavour: ModItem
    {

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.maxStack = 30;
            item.value = 20000;
            item.rare = 9;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Redigit's Favour");
            Tooltip.SetDefault("");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 6));
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.position, new Vector3(.6f, .4f, .2f));
        }

    }
}
