using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCFBattles.NPCs.Devs
{
    class Redigit : ModNPC
    {

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 4000;
            npc.damage = 1;
            npc.defense = 50;
            npc.knockBackResist = 0f;
            npc.width = 18;
            npc.height = 40;
            npc.boss = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.lavaImmune = true;
            music = MusicID.LunarBoss;
            Main.npcFrameCount[npc.type] = 9;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Redigit");
        }

        public override void AI()
        {

            // ai[0] = stages.
            // 0 = flying.
            // 1 = landing.
            // 2 = grounded
            // ai[1] = stage timer.
            //

            // Let there be light.

            Lighting.AddLight(npc.position, 1f, 1f, 1f);

            npc.TargetClosest();

            // Flying stage.

            if (npc.ai[0] == 0)
            {

                float horMoveSpeed = 0.07f;
                float vertMoveSpeed = 0.15f;
                float maxMoveSpeed = 10f;

                // Going up.

                if (npc.Center.Y > Main.player[npc.target].Center.Y - 200)
                {
                    if (npc.velocity.Y > 0)
                        npc.velocity.Y *= 0.8f;

                    npc.velocity.Y -= vertMoveSpeed;

                    if (npc.velocity.Y < -maxMoveSpeed)
                        npc.velocity.Y = -maxMoveSpeed;
                }

                // Going down.

                if (npc.Center.Y < Main.player[npc.target].Center.Y - 200)
                {
                    if (npc.velocity.Y < 0)
                        npc.velocity.Y *= 0.8f;

                    npc.velocity.Y += vertMoveSpeed;

                    if (npc.velocity.Y > maxMoveSpeed)
                        npc.velocity.Y = maxMoveSpeed;
                }

                // Going left.

                if (npc.Center.X > Main.player[npc.target].Center.X + 200)
                {
                    if (npc.velocity.X > 0)
                        npc.velocity.X *= 0.98f;

                    npc.velocity.X -= horMoveSpeed;

                    if (npc.velocity.X < -maxMoveSpeed)
                        npc.velocity.X = -maxMoveSpeed;
                }

                // Going right.

                if (npc.Center.X < Main.player[npc.target].Center.X + 200)
                {
                    if (npc.velocity.X < 0)
                        npc.velocity.X *= 0.98f;

                    npc.velocity.X += horMoveSpeed;

                    if (npc.velocity.X > maxMoveSpeed)
                        npc.velocity.X = maxMoveSpeed;
                }

                // When velocities are too low, set them to zero (prevents jittery movement).

                if (npc.velocity.X > -0.05f && npc.velocity.X < 0.05f)
                    npc.velocity.X = 0;
                if (npc.velocity.Y > -0.05f && npc.velocity.Y < 0.05f)
                    npc.velocity.Y = 0;

                // Timer for the stages.


                if (npc.ai[1] == 600)
                {
                    YoyoMaelstrom();
                }
                else if (npc.ai[1] == 1200)
                {
                    YoyoRang();
                }
                else if (npc.ai[1] == 1800)
                {
                    YoyoSling();
                }
                else if (npc.ai[1] == 2400)
                {
                    npc.ai[0] = 1;
                    npc.ai[1] = -1;
                    npc.netUpdate = true;
                }
                else if (npc.ai[1] % 600 != 0 && npc.ai[1] % 600 != 5 && npc.ai[1] % 600 != 10 && (npc.ai[1] % 150 == 0 || npc.ai[1] % 150 == 5 || npc.ai[1] % 150 == 10))
                {                    
                    YoyoBurst();
                }

                npc.ai[1]++;
            }

            // Landing stage.

            else if (npc.ai[0] == 1)
            {
                
                float vertMoveSpeed = 1.2f;
                float vertMaxMoveSpeed = 5f;
                float horMinMoveSpeed = 0.2f;
                npc.noTileCollide = false;

                // Going down.

                if (npc.velocity.Y < 0)
                    npc.velocity.Y *= 0.8f;

                npc.velocity.Y += vertMoveSpeed;

                if (npc.velocity.Y > vertMaxMoveSpeed)
                    npc.velocity.Y = vertMaxMoveSpeed;


                // Brakes.

                npc.velocity.X *= 0.98f;

                if (npc.velocity.X < horMinMoveSpeed && npc.velocity.X > 0.0f)
                    npc.velocity.X = horMinMoveSpeed;
                if (npc.velocity.X > -horMinMoveSpeed && npc.velocity.X < 0.0f)
                    npc.velocity.X = -horMinMoveSpeed;

                if (npc.collideY == true)
                {
                    npc.velocity = Vector2.Zero;
                    npc.ai[0] = 2;
                }

                Vector2 targetDirection = npc.Center - Main.player[npc.target].Center;
                float targetDistance = (float)Math.Sqrt(targetDirection.X * targetDirection.X + targetDirection.Y * targetDirection.Y);

                if (targetDistance > 1000f)
                {
                    npc.ai[0] = 0;
                    npc.noTileCollide = true;
                }
            }

            // Grounded stage.

            else if (npc.ai[0] == 2)
            {
                Vector2 targetDirection = npc.Center - Main.player[npc.target].Center;
                float targetDistance = (float)Math.Sqrt(targetDirection.X * targetDirection.X + targetDirection.Y * targetDirection.Y);

                if(npc.ai[1] % 30 == 0 && npc.ai[1] < 180)
                {
                    YoyoSling();
                }
                if(npc.ai[1] == 300)
                {
                    npc.ai[0] = 0;
                    npc.ai[1] = -1;
                    npc.noTileCollide = true;
                }

                if (targetDistance > 1000f)
                {
                    npc.ai[0] = 0;
                    npc.noTileCollide = true;
                }

                npc.ai[1]++;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            if (npc.frameCounter == 24 && npc.ai[0] == 0)
            {
                Main.PlaySound(2, npc.position, 32);
            }
            npc.frameCounter %= 24;
            if (npc.ai[0] == 0)
            {
                if (npc.velocity.Y < 2.5f)
                    npc.frame.Y = frameHeight * (((int)npc.frameCounter / 8) + 1);
                else
                    npc.frame.Y = frameHeight * (((int)npc.frameCounter / 8) + 5);
            }
            if (npc.ai[0] == 1)
                npc.frame.Y = frameHeight * 7;
            if (npc.ai[0] == 2)
                npc.frame.Y = 0;

        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = 499;
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RedigitFavour"));
        }

        public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
        {          
            spriteEffects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return true;
        }

        private void YoyoSling()
        {
            Main.PlaySound(2, npc.position, 8); // Plays a 'conjure' sound.
            Projectile.NewProjectile(npc.position.X, npc.position.Y, npc.velocity.X, npc.velocity.X, mod.ProjectileType("YoyoSling"), 1, 10, Main.myPlayer, npc.whoAmI, npc.direction);
        }

        private void YoyoMaelstrom()
        {
            Main.PlaySound(2, npc.position, 8); // Plays a 'conjure' sound.
            Projectile.NewProjectile(npc.position.X, npc.position.Y, npc.velocity.X, npc.velocity.X, mod.ProjectileType("YoyoMaelstrom"), 1, 10, Main.myPlayer, npc.whoAmI, npc.direction);
        }

        private void YoyoRang()
        {
            Projectile.NewProjectile(npc.position.X, npc.position.Y, npc.velocity.X, npc.velocity.X, mod.ProjectileType("YoyoRang"), 1, 10, Main.myPlayer, npc.whoAmI, 0);
        }

        private void YoyoBurst()
        {
            float burstSpeed = 20f;

            Vector2 velocity = Main.player[npc.target].Center - npc.Center;
            velocity.Normalize();
            velocity *= burstSpeed;
            velocity += npc.velocity;

            Main.PlaySound(2, npc.position, 19); // Plays a 'throw' sound.
            Projectile.NewProjectile(npc.position.X, npc.position.Y, velocity.X, velocity.Y, mod.ProjectileType("YoyoMaelstromChild"), 1, 10);
        }
    }
}

