using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Items.Dyes
{
    class TestDye : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oktoberfest Dye");
        }
    }
}
