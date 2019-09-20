using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using KazDyePack.Items.Dyes;

namespace KazDyePack
{
    class DyePackPlayer : ModPlayer
    {
        public override void GetDyeTraderReward(List<int> rewardPool)
        {
            rewardPool.Add(mod.ItemType<LysergicAcidDye>());
        }
    }
}
