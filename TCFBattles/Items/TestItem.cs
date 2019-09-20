using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.ID;

namespace TCFBattles.Items
{
    class TestItem : ModItem
    {
        int shaderId = 0;

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.useAnimation = 10;
            item.useStyle = 4;
            item.useTime = 30;
            item.UseSound = SoundID.Item39;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test Item");
        }

        public override bool UseItem(Player player)
        {
            //Main.PlaySound(15, player.position, 0);
            ////Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("YoyoSling"), 80, 10f, Main.myPlayer, 0, Main.myPlayer);
            ////Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("YoyoMaelstrom"), 0, 0);
            //// NPC.NewNPC((int)player.position.X + 2000, (int)player.position.Y, mod.NPCType("Redigit"));
            //NPC.NewNPC((int)player.position.X - 2000, (int)player.position.Y, mod.NPCType("Cenx"));
            //return true;

            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (shaderId > 0)
            {
                string filter = "Shockwave" + shaderId;
                Filters.Scene[filter].GetShader().UseProgress(Main.GlobalTime % 5).UseOpacity(100f);
                return;
            }  

            if (!Filters.Scene["Shockwave1"].IsActive())
            {
                shaderId = 1;
                Filters.Scene.Activate("Shockwave1", item.Center).GetShader().UseColor(1, 5, 3).UseTargetPosition(item.Center);
            }
            else if(!Filters.Scene["Shockwave2"].IsActive())
            {
                shaderId = 2;
                Filters.Scene.Activate("Shockwave2", item.Center).GetShader().UseColor(1, 5, 3).UseTargetPosition(item.Center);
            }

            
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
