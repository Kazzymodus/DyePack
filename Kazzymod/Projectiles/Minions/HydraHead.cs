using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    public abstract class HydraHead : ModProjectile
    {
        private static readonly Vector2 BaseHeadOffset = new Vector2(30, -40);

        protected virtual TargetMode TargetPriority { get; } = TargetMode.Random; 

        // protected virtual float MinAttackRange { get; } = 50;
        protected virtual float MaxAttackRange { get; } = 500;
        protected virtual int AttackCooldown { get; } = 120;
        protected virtual int AttackDuration { get; } = 1;

        protected virtual bool ContinueAttackWhenTargetDead { get; } = false;

        protected int HeadType{ get; private set; }
        protected int BodyId { get; private set; }

        protected int Target
        {
            get { return (int)projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        protected bool HasTarget
        {
            get { return projectile.ai[0] >= 0; }
        }

        protected int Timer
        {
            get { return (int)projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.minion = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1f;
            projectile.friendly = true;
            projectile.timeLeft *= 5;
            projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
        }

        public override void AI()
        {
            // projectile.ai[0] // Attack data
            // 0-7 = target
            // 8 = attack in progress (0 = no, 1 = yes)
            // 9 = attack type (0 = melee, 1 = ranged)
            // projectile.ai[1] // Timer

            KazzyPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<KazzyPlayer>();
            Projectile body = Main.projectile[BodyId];

            // Check if the Hydra Buff is active. If not, kill the Hydra.

            if (modPlayer.hydraMinion)
            {
                projectile.timeLeft = 2;
            }
            else
            {
                Kill(projectile.timeLeft);
                return;
            }

            // Move the head with the body, and space out the necks.

            projectile.direction = body.direction;

            Vector2 headOffset = BaseHeadOffset;
            headOffset.X -= 10 * HeadType;
            headOffset.X *= projectile.direction;

            projectile.position = body.Center + (projectile.Size * 1) + headOffset;

            //{ // DEBUG
            //    if (Timer++ % 10 == 0)
            //    {
            //        Attack();
            //    }
            //    return;
            //} // DEBUG

            // Unless we are ready to attack and waiting for a target, progress the timer.

            if (!(Timer == 0 && !HasTarget))
            {
                projectile.ai[1]--;
            }

            // Once the cooldown has reached 0, start attacking.

            if (Timer == 0)
            {
                // Find a target.

                if (GetTarget(modPlayer.player))
                {
                    // If we found a target, start attacking it

                    PreAttack();
                    Attack();
                }
            }
            else if (Timer < 0 && (HasTarget || ContinueAttackWhenTargetDead))
            {
                // If our attack is over, reset it.

                if (Timer <= -AttackDuration + 1)
                {
                    ResetAttack();
                }
                else
                {

                    // Check if the target is still active.

                    if ((HasTarget && Main.npc[Target].active && Main.npc[Target].life > 0) || ContinueAttackWhenTargetDead)
                    {
                        Attack();
                    }
                    else
                    {
                        // If not, reset the attack.

                        ResetAttack();
                    }
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return false;
        }

        private void ResetAttack()
        {
            Target = -1;
            Timer = AttackCooldown;
        }

        protected void AbortAttack()
        {
            Target = -1;
            Timer = 0;
        }

        protected void ClearTarget()
        {
            Target = -1;
        }

        protected bool GetTarget(Player owner)
        {
            if (TargetPriority == TargetMode.Random)
            {
                return GetRandomTarget(owner);
            }

            float bestDistance = (TargetPriority == TargetMode.ClosestToHydra || TargetPriority == TargetMode.ClosestToPlayer) ? float.MaxValue : 0;
            int target = -1;

            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal)
                {
                    float hydraToEnemyX = npc.Center.X - projectile.Center.X;
                    float hydraToEnemyY = npc.Center.Y - projectile.Center.Y;
                    float hydraToEnemySq = hydraToEnemyX * hydraToEnemyX + hydraToEnemyY * hydraToEnemyY;

                    if (hydraToEnemySq < MaxAttackRange * MaxAttackRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        if (TargetPriority == TargetMode.ClosestToHydra)
                        {
                            if (hydraToEnemySq < bestDistance)
                            {
                                bestDistance = hydraToEnemySq;
                                target = i;
                            }

                            continue;
                        }
                        
                        if (TargetPriority == TargetMode.FurthestFromHydra)
                        {
                            if (hydraToEnemySq > bestDistance)
                            {
                                bestDistance = hydraToEnemySq;
                                target = i;
                            }

                            continue;
                        }

                        float playerToEnemyX = npc.Center.X - owner.Center.X;
                        float playerToEnemyY = npc.Center.Y - owner.Center.Y;
                        float playerToEnemySq = playerToEnemyX * playerToEnemyX + playerToEnemyY + playerToEnemyY;

                        if (TargetPriority == TargetMode.ClosestToPlayer)
                        {
                            if (playerToEnemySq < bestDistance)
                            {
                                bestDistance = playerToEnemySq;
                                target = i;
                            }

                            continue;
                        }

                        if (TargetPriority == TargetMode.FurthestFromPlayer)
                        {
                            if (playerToEnemySq > bestDistance)
                            {
                                bestDistance = playerToEnemySq;
                                target = i;
                            }

                            continue;
                        }
                    }
                }
            }

            Target = target;
            return target > -1;
        }

        protected bool GetRandomTarget(Player owner)
        {
            List<int> validTargets = new List<int>();
            List<float> distances = new List<float>();

            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal)
                {
                    float hydraToEnemyX = npc.Center.X - projectile.Center.X;
                    float hydraToEnemyY = npc.Center.Y - projectile.Center.Y;
                    float hydraToEnemySq = hydraToEnemyX * hydraToEnemyX + hydraToEnemyY * hydraToEnemyY;

                    if (hydraToEnemySq < MaxAttackRange * MaxAttackRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        validTargets.Add(i);
                        distances.Add(hydraToEnemySq);
                    }
                }
            }

            if (validTargets.Count > 0)
            {
                int index = Main.rand.Next(validTargets.Count);
                Target = validTargets[index];
                return true;
            }

            Target = -1;
            return false;
        }

        public void Initialise()
        {
            Target = -1;
            HeadType = Main.player[projectile.owner].GetModPlayer<KazzyPlayer>().hydraCount;

            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == mod.ProjectileType<HydraBody>())
                {
                    BodyId = i;
                    break;
                }
            }
        }

        protected virtual void PreAttack()
        {
        }

        protected abstract void Attack();

        protected enum TargetMode
        {
            Random,
            FurthestFromPlayer,
            ClosestToPlayer,
            FurthestFromHydra,
            ClosestToHydra,
        }
    }
}
