namespace KazDyePack
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Terraria;
    using Terraria.Graphics.Effects;
    using Terraria.Graphics.Shaders;
    using Terraria.ID;
    using Terraria.ModLoader;

    public class KazDyePack : Mod
	{
        public KazDyePack()
        {
            Properties = new ModProperties()
            {
                Autoload = true
            };
        }

        public override void Load()
        {
            Ref<Effect> dyeRef = new Ref<Effect>(GetEffect("Effects/KazDyes"));

            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Gem.RubyDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(1.5f, 0.15f, 0f);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Gem.TopazDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(1.7f, 0.75f, 0f);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Gem.EmeraldDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(0f, 0.8f, 0f);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Gem.SapphireDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(0f, 0f, 1.1f);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Gem.AmethystDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(1f, 0f, 1.4f);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Gem.DiamondDye>(), new ArmorShaderData(dyeRef, "ArmorGem")).UseImage("Images/Misc/noise").UseColor(0.8f, 0.65f, 1.2f);

            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Radar.BrightRadarDye>(), new ArmorShaderData(dyeRef, "ArmorRadar")).UseColor(1.7f, 1.7f, 1.7f);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Radar.RedRadarDye>(), new ArmorShaderData(dyeRef, "ArmorRadar")).UseColor(0.8f, 0, 0);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Radar.GreenRadarDye>(), new ArmorShaderData(dyeRef, "ArmorRadar")).UseColor(0, 0.8f, 0);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Radar.BlueRadarDye>(), new ArmorShaderData(dyeRef, "ArmorRadar")).UseColor(0, 0, 0.8f);

            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Wave.RisingFlameDye>(), new ArmorShaderData(dyeRef, "ArmorWave")).UseColor(1.8f, 1, 0).UseSecondaryColor(0.3f, 0.3f, 0.3f);
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.Wave.RisingTideDye>(), new ArmorShaderData(dyeRef, "ArmorWave")).UseColor(0, 1, 1.8f).UseSecondaryColor(0.35f, 0.25f, 0.4f);

            GameShaders.Armor.BindShader(ItemType<Items.Dyes.HazeDye>(), new ArmorShaderData(dyeRef, "ArmorHaze"));
            GameShaders.Armor.BindShader(ItemType<Items.Dyes.LysergicAcidDye>(), new ArmorShaderData(dyeRef, "ArmorLysergicAcid"));

        }
    }
}