using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCFBattles
{
	public class TCFBattles : Mod
	{
        static Effect customDyeEffect;

        public TCFBattles()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
            };
        }

        public override void Load()
		{
            if (!Main.dedServ)
            {
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/ShockwaveEffect"));

                Filters.Scene["Shockwave1"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave1"].Load();
                Filters.Scene["Shockwave2"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave2"].Load();
                Filters.Scene["Shockwave3"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave3"].Load();
            }
        }

		public override void Unload()
		{
			
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.Javelin, 99);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.VortexBeater);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.EndlessMusketPouch);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.SuperHealingPotion, 10);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.ChlorophyteBullet, 999);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.ShroomiteBreastplate);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.ShroomiteMask);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.ShroomiteLeggings);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.CenxsTiara);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.CenxsDress);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.CenxsDressPants);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.CenxsWings);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.Bomb, 99);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.DesertFossil);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemType("GroundZero"));
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemType("GroundZeroAmmo"), 99);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.ProximityMineLauncher);
            recipe.AddRecipe(); recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemID.RocketIV, 99);
            recipe.AddRecipe();
            recipe = new ModRecipe(this);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(ItemType<Items.Dyes.TestDye>(), 3);
            recipe.AddRecipe();
        }
    }
}