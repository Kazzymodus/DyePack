using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace TCFBattles.Items.Crafted
{
    class DesignerFavour : ModItem
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
            DisplayName.SetDefault("Soul of the Designers");
            Tooltip.SetDefault("");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 6));
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.position, new Vector3(.6f, .4f, .2f));
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RedigitFavour"));
            recipe.AddIngredient(mod.ItemType("CenxFavour"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
