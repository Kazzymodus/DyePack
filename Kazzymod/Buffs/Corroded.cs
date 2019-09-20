using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Kazzymod.NPCs;

namespace Kazzymod.Buffs
{
    class Corroded : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Corroded");
            Description.SetDefault("Reduced defense");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense -= 5;

            if(npc.defense < 0)
            {
                npc.defense = 0;
            }

            npc.GetGlobalNPC<KazzyNPC>().corroded = true;
        }
    }
}
